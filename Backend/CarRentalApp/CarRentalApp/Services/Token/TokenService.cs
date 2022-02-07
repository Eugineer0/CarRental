using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRentalApp.Configuration.JWT.Access;
using Microsoft.IdentityModel.Tokens;
using CarRentalApp.Models.Entities;
using CarRentalApp.Configuration.JWT;
using Microsoft.Extensions.Options;
using CarRentalApp.Configuration.JWT.Refresh;
using CarRentalApp.Exceptions;
using CarRentalApp.Models.DTOs;
using CarRentalApp.Repositories;
using Microsoft.Net.Http.Headers;

namespace CarRentalApp.Services.Token
{
    public class TokenService
    {
        const int TOKEN_START_INDEX = 7;

        private readonly RefreshTokenRepository _refreshTokenRepository;

        private readonly AccessJwtConfig _accessJwtConfig;
        private readonly RefreshJwtConfig _refreshJwtConfig;

        public TokenService(
            RefreshTokenRepository refreshTokenRepository,
            IOptions<AccessJwtConfig> accessJwtConfig,
            IOptions<RefreshJwtConfig> refreshJwtConfig
        )
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accessJwtConfig = accessJwtConfig.Value;
            _refreshJwtConfig = refreshJwtConfig.Value;
        }

        public static SecurityKey GetKey(GenerationParameters jwtParams)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtParams.Secret));
        }

        public JwtSecurityToken GenerateAccessToken(User user)
        {
            var accessJwtGenerationParams = _accessJwtConfig.GenerationParameters;

            var jwtClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.Role.ToString())
            };

            return new JwtSecurityToken(
                issuer: accessJwtGenerationParams.Issuer,
                audience: accessJwtGenerationParams.Audience,
                claims: jwtClaims,
                expires: DateTimeOffset.Now.UtcDateTime.AddSeconds(accessJwtGenerationParams.LifeTimeSeconds),
                signingCredentials: GetJWTCredentials(accessJwtGenerationParams)
            );
        }

        public JwtSecurityToken GenerateRefreshToken(User user)
        {
            var refreshJwtGenerationParams = _refreshJwtConfig.GenerationParameters;

            var jwtClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };

            return new JwtSecurityToken(
                claims: jwtClaims,
                expires: DateTimeOffset.Now.UtcDateTime.AddSeconds(refreshJwtGenerationParams.LifeTimeSeconds),
                signingCredentials: GetJWTCredentials(refreshJwtGenerationParams)
            );
        }

        /// <exception cref="SharedException">Token not found by <paramref name="refreshTokenDto"/>.</exception>
        public async Task<RefreshToken> PopTokenAsync(RefreshTokenDTO refreshTokenDTO)
        {
            var token = await _refreshTokenRepository.GetByTokenStringAsync(refreshTokenDTO.Token);
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

            await _refreshTokenRepository.DeleteAsync(token);

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

            await _refreshTokenRepository.InsertAsync(refreshToken);

            return refreshToken;
        }

        private Task InvalidateRelatedRefreshTokensAsync(RefreshTokenDTO refreshTokenDTO)
        {
            var userId = GetTokenOwnerId(refreshTokenDTO.Token);

            return _refreshTokenRepository.DeleteRelatedTokensByUserIdAsync(userId);
        }

        private TokenValidationParameters GetRefreshValidationParams()
        {
            var refreshJwtValidationParams = _refreshJwtConfig.ValidationParameters;
            refreshJwtValidationParams.IssuerSigningKey = GetKey(_refreshJwtConfig.GenerationParameters);

            return refreshJwtValidationParams;
        }

        /// <exception cref="SharedException">Claim 'sub' of <paramref name="tokenString"/> not found.</exception>
        /// <exception cref="SharedException">Claim 'sub' value is invalid.</exception>
        private Guid GetTokenOwnerId(string tokenString)
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
                    "Claim 'sub' not found"
                );
            }

            if (!Guid.TryParse(userIdString, out var userId))
            {
                throw new SharedException(
                    ErrorTypes.Invalid,
                    "Invalid 'sub' claim value"
                );
            }

            return userId;
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