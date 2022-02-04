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
using CarRentalApp.Models.DTOs.Requests;
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

        /// <exception cref="GeneralException"><paramref name="headers"/> do not contain expected header .</exception>
        public RefreshTokenDTO GetTokenFromHeaders(IHeaderDictionary headers)
        {
            var tokenHeader = headers[HeaderNames.Authorization];
            if (tokenHeader.Count == 0)
            {
                throw new GeneralException(
                    ErrorTypes.NotFound,
                    "Authorization header not found",
                    null
                );
            }

            return new RefreshTokenDTO()
            {
                Token = tokenHeader.ToString()[TOKEN_START_INDEX..]
            };
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
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
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

        /// <exception cref="GeneralException">Token not found by <paramref name="refreshTokenDto"/>.</exception>
        public async Task<RefreshToken> PopTokenAsync(RefreshTokenDTO refreshTokenDto)
        {
            var token = await _refreshTokenRepository.GetByTokenStringAsync(refreshTokenDto.Token);
            if (token == null)
            {
                await InvalidateRelatedRefreshTokensAsync(refreshTokenDto);
                throw new GeneralException(
                    ErrorTypes.NotFound,
                    "Refresh token not found, all sessions will be closed",
                    null
                );
            }

            await _refreshTokenRepository.DeleteAsync(token);

            return token;
        }

        public async Task<bool> CheckIfValidAsync(RefreshTokenDTO refreshTokenDto)
        {
            var refreshJwtValidationParams = GetRefreshValidationParams();

            try
            {
                new JwtSecurityTokenHandler().ValidateToken(
                    refreshTokenDto.Token,
                    refreshJwtValidationParams,
                    out var securityToken
                );
            }
            catch (SecurityTokenExpiredException)
            {
                try
                {
                    await PopTokenAsync(refreshTokenDto);
                }
                catch (GeneralException)
                {
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
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
        
        public async Task InvalidateRefreshTokenAsync(RefreshTokenDTO refreshTokenDto)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenStringAsync(refreshTokenDto.Token);
            if (refreshToken == null)
            {
                await InvalidateRelatedRefreshTokensAsync(refreshTokenDto);
                throw new GeneralException(
                    ErrorTypes.NotFound,
                    "Refresh token not found, all related sessions will be closed",
                    null
                );
            }
            
            await _refreshTokenRepository.DeleteAsync(refreshToken);
        }

        public Task InvalidateRelatedRefreshTokensAsync(RefreshTokenDTO refreshTokenDto)
        {
            var userId = GetTokenOwnerId(refreshTokenDto.Token);

            return _refreshTokenRepository.DeleteRelatedTokensByUserIdAsync(userId);
        }

        private TokenValidationParameters GetRefreshValidationParams()
        {
            var refreshJwtValidationParams = _refreshJwtConfig.ValidationParameters;
            refreshJwtValidationParams.IssuerSigningKey = GetKey(_refreshJwtConfig.GenerationParameters);

            return refreshJwtValidationParams;
        }

        /// <exception cref="GeneralException">Sub claim of <paramref name="tokenString"/> not found.</exception>
        /// <exception cref="GeneralException">Sub claim value is invalid.</exception>
        private Guid GetTokenOwnerId(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenString);

            var userIdString = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?
                .Value;

            if (userIdString == null)
            {
                throw new GeneralException(
                    ErrorTypes.NotFound,
                    "Required claim not found",
                    null
                );
            }

            if (!Guid.TryParse(userIdString, out var userId))
            {
                throw new GeneralException(
                    ErrorTypes.Invalid,
                    "Invalid claim value",
                    null
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