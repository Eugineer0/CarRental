﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRentalApp.Configuration.JWT;
using CarRentalApp.Configuration.JWT.Access;
using CarRentalApp.Configuration.JWT.Refresh;
using CarRentalApp.Contexts;
using CarRentalApp.Exceptions;
using CarRentalApp.Models.BLL;
using CarRentalApp.Models.DAL;
using CarRentalApp.Models.WEB.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalApp.Services
{
    public class TokenService
    {
        private readonly AccessJwtConfig _accessJwtConfig;
        private readonly RefreshJwtConfig _refreshJwtConfig;
        private readonly CarRentalDbContext _carRentalDbContext;

        public TokenService(
            IOptions<AccessJwtConfig> accessJwtConfig,
            IOptions<RefreshJwtConfig> refreshJwtConfig,
            CarRentalDbContext carRentalDbContext
        )
        {
            _carRentalDbContext = carRentalDbContext;
            _accessJwtConfig = accessJwtConfig.Value;
            _refreshJwtConfig = refreshJwtConfig.Value;
        }

        /// <summary>
        /// Gets string secret key from <paramref name="jwtParams"/> and returns its symmetric representation.
        /// </summary>
        /// <param name="jwtParams">params, containing string secret key.</param>
        /// <returns>Symmetric representation of string key.</returns>
        public static SecurityKey GetKey(GenerationParameters jwtParams)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtParams.Secret));
        }

        /// <summary>
        /// Removes token model found by <paramref name="refreshAccessRequest"/> and returns it.
        /// </summary>
        /// <param name="refreshAccessRequest">ejected token prototype.</param>
        /// <returns>token model, removed from database.</returns>
        /// <exception cref="SharedException">Token not found by <paramref name="refreshAccessRequest"/>. For safety reasons all related tokens will be revoked</exception>
        public async Task<RefreshToken> PopTokenAsync(string refreshTokenString)
        {
            var token = await _carRentalDbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token.Equals(refreshTokenString));
            if (token == null)
            {
                var userId = Guid.Empty;
                SharedException? innerException = null;

                try
                {
                    userId = GetUserId(refreshTokenString);
                }
                catch (SharedException exception)
                {
                    innerException = exception;
                }

                await RevokeRefreshTokensByIdAsync(userId);

                throw new SharedException(
                    ErrorTypes.AuthFailed,
                    "Invalid refresh token",
                    "Refresh token not found, all related sessions will be closed",
                    innerException
                );
            }

            _carRentalDbContext.RefreshTokens.Remove(token);
            await _carRentalDbContext.SaveChangesAsync();

            return token;
        }

        /// <summary>
        /// Validates token 'expiration' claim.
        /// </summary>
        /// <param name="tokenString">token to be validated.</param>
        /// <exception cref="SharedException"><paramref name="tokenString"/> is expired.</exception>
        public void ValidateTokenLifetime(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenString);
            var expires = jwtSecurityToken.ValidTo;

            var refreshJwtValidationParams = GetRefreshValidationParams();

            try
            {
                Validators.ValidateLifetime(null, expires, jwtSecurityToken, refreshJwtValidationParams);
            }
            catch (SecurityTokenExpiredException exception)
            {
                throw new SharedException(
                    ErrorTypes.AuthFailed,
                    "Invalid refresh token",
                    "Refresh token expired",
                    exception
                );
            }
        }

        /// <summary>
        /// Generates and returns pair of user-oriented tokens, saves refresh token to database.
        /// </summary>
        /// <param name="user">user to get access.</param>
        /// <returns>Pair of auth tokens.</returns>
        public async Task<AccessModel> GetAccessAsync(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken(user);

            await StoreRefreshTokenAsync(refreshToken, user.Id);

            return new AccessModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        /// <summary>
        /// Gets user id from <paramref name="tokenString"/> and returns it.
        /// </summary>
        /// <param name="tokenString">token to be parsed.</param>
        /// <returns>User id, stored in 'sub' claim.</returns>
        /// <exception cref="SharedException">Claim 'sub' of <paramref name="tokenString"/> not found.</exception>
        /// <exception cref="SharedException">Claim 'sub' value is invalid.</exception>
        public Guid GetUserId(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenString);
            var userIdString = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)
                ?.Value;

            if (userIdString == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "Invalid token",
                    "Claim 'sub' not found"
                );
            }

            if (!Guid.TryParse(userIdString, out var userId))
            {
                throw new SharedException(
                    ErrorTypes.Invalid,
                    "Invalid token",
                    "Invalid 'sub' claim value"
                );
            }

            return userId;
        }

        /// <summary>
        /// Generates long-term token to refresh access.
        /// </summary>
        /// <param name="user">token owner.</param>
        /// <returns>Generated refresh token.</returns>
        public string GenerateRefreshToken(User user)
        {
            var refreshJwtGenerationParams = _refreshJwtConfig.GenerationParameters;

            var jwtClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var refreshToken = new JwtSecurityToken(
                claims: jwtClaims,
                expires: DateTimeOffset.Now.UtcDateTime.AddSeconds(refreshJwtGenerationParams.LifeTimeSeconds),
                signingCredentials: GetJWTCredentials(refreshJwtGenerationParams)
            );

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }

        /// <summary>
        /// Generates short-term token to access resources according to <paramref name="user"/> roles.
        /// </summary>
        /// <param name="user">token owner.</param>
        /// <returns>Generated access token.</returns>
        private string GenerateAccessToken(User user)
        {
            var accessJwtGenerationParams = _accessJwtConfig.GenerationParameters;

            var jwtClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            foreach (var role in user.Roles.Select(r => r.Role))
            {
                jwtClaims.Add(new Claim("role", role.ToString()));
            }

            var accessToken = new JwtSecurityToken(
                issuer: accessJwtGenerationParams.Issuer,
                audience: accessJwtGenerationParams.Audience,
                claims: jwtClaims,
                expires: DateTimeOffset.Now.UtcDateTime.AddSeconds(accessJwtGenerationParams.LifeTimeSeconds),
                signingCredentials: GetJWTCredentials(accessJwtGenerationParams)
            );

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        /// <summary>
        /// Saves refresh token model based on <paramref name="refreshTokenString"/> and <paramref name="userId"/>.
        /// </summary>
        /// <param name="refreshTokenString">token prototype to be saved.</param>
        /// <param name="userId">token model field.</param>
        private Task StoreRefreshTokenAsync(string refreshTokenString, Guid userId)
        {
            var refreshToken = new RefreshToken()
            {
                Token = refreshTokenString,
                UserId = userId
            };

            _carRentalDbContext.RefreshTokens.Add(refreshToken);

            return _carRentalDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Revokes all stored tokens with specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">token model field.</param>
        private async Task RevokeRefreshTokensByIdAsync(Guid userId)
        {
            await _carRentalDbContext.RefreshTokens
                .Where(t => t.UserId == userId)
                .ForEachAsync(token => _carRentalDbContext.RefreshTokens.Remove(token));

            await _carRentalDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gathers token validation params from JWT config and secret key from token generation params and returns it.
        /// </summary>
        /// <returns>Gathered token validation params.</returns>
        private TokenValidationParameters GetRefreshValidationParams()
        {
            var refreshJwtValidationParams = _refreshJwtConfig.ValidationParameters;
            refreshJwtValidationParams.IssuerSigningKey = GetKey(_refreshJwtConfig.GenerationParameters);

            return refreshJwtValidationParams;
        }

        /// <summary>
        /// Gets signing credentials for token generation from passed token generation params.
        /// </summary>
        /// <param name="jwtParams">params, which contain string secret key.</param>
        /// <returns>Signing credentials for token generation.</returns>
        private SigningCredentials GetJWTCredentials(GenerationParameters jwtParams)
        {
            return new SigningCredentials(
                GetKey(jwtParams),
                SecurityAlgorithms.HmacSha256Signature
            );
        }
    }
}