using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRentalApp.Configuration.JWT.Access;
using Microsoft.IdentityModel.Tokens;
using CarRentalApp.Models.Entities;
using CarRentalApp.Configuration.JWT;
using Microsoft.Extensions.Options;
using CarRentalApp.Configuration.JWT.Refresh;
using CarRentalApp.DAOs;
using CarRentalApp.Exceptions;
using CarRentalApp.Models.DTOs;

namespace CarRentalApp.Services.Token
{
    public class TokenService
    {
        private readonly RefreshTokenDAO _refreshTokenDAO;

        private readonly AccessJwtConfig _accessJwtConfig;
        private readonly RefreshJwtConfig _refreshJwtConfig;

        public TokenService(
            RefreshTokenDAO refreshTokenDAO,
            IOptions<AccessJwtConfig> accessJwtConfig,
            IOptions<RefreshJwtConfig> refreshJwtConfig
        )
        {
            _refreshTokenDAO = refreshTokenDAO;
            _accessJwtConfig = accessJwtConfig.Value;
            _refreshJwtConfig = refreshJwtConfig.Value;
        }

        public static SecurityKey GetKey(GenerationParameters jwtParams)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtParams.Secret));
        }

        public string GenerateAccessToken(User user)
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

        /// <exception cref="SharedException">Token not found by <paramref name="refreshTokenDto"/>.</exception>
        public async Task<RefreshToken> PopTokenAsync(RefreshTokenDTO refreshTokenDTO)
        {
            var token = await _refreshTokenDAO.GetByTokenStringAsync(refreshTokenDTO.Token);
            if (token == null)
            {
                SharedException? innerException = null;

                try
                {
                    await InvalidateRelatedRefreshTokensAsync(refreshTokenDTO);
                }
                catch (SharedException exception)
                {
                    innerException = exception;
                }

                throw new SharedException(
                    ErrorTypes.AuthFailed,
                    "Invalid refresh token",
                    "Refresh token not found, all related sessions will be closed",
                    innerException
                );
            }

            await _refreshTokenDAO.DeleteAsync(token);

            return token;
        }

        /// <exception cref="SharedException"><paramref name="refreshTokenDto"/> is expired.</exception>
        public void ValidateTokenLifetime(RefreshTokenDTO refreshTokenDTO)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(refreshTokenDTO.Token);
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

        public async Task<RefreshToken> StoreRefreshTokenAsync(string refreshTokenString, User user)
        {
            var refreshToken = new RefreshToken()
            {
                Token = refreshTokenString,
                UserId = user.Id
            };

            await _refreshTokenDAO.InsertAsync(refreshToken);

            return refreshToken;
        }

        /// <exception cref="SharedException">Claim 'sub' of <paramref name="tokenString"/> not found.</exception>
        /// <exception cref="SharedException">Claim 'sub' value is invalid.</exception>
        public Guid GetUserId(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenString);
            var userIdString = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?
                .Value;

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

        private Task InvalidateRelatedRefreshTokensAsync(RefreshTokenDTO refreshTokenDTO)
        {
            var userId = GetUserId(refreshTokenDTO.Token);

            return _refreshTokenDAO.DeleteTokensByUserIdAsync(userId);
        }

        private TokenValidationParameters GetRefreshValidationParams()
        {
            var refreshJwtValidationParams = _refreshJwtConfig.ValidationParameters;
            refreshJwtValidationParams.IssuerSigningKey = GetKey(_refreshJwtConfig.GenerationParameters);

            return refreshJwtValidationParams;
        }

        private SigningCredentials GetJWTCredentials(GenerationParameters jwtParams)
        {
            return new SigningCredentials(
                GetKey(jwtParams),
                SecurityAlgorithms.HmacSha256Signature
            );
        }
    }
}