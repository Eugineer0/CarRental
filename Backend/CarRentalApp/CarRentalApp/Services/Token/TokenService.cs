using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRentalApp.Configuration.JWT.Access;
using Microsoft.IdentityModel.Tokens;
using CarRentalApp.Models.Entities;
using CarRentalApp.Configuration.JWT;
using Microsoft.Extensions.Options;
using CarRentalApp.Configuration.JWT.Refresh;
using CarRentalApp.Exceptions.BLL;
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

        public string GetTokenStringFromHeaders(IHeaderDictionary headers)
        {
            return headers[HeaderNames.Authorization].ToString()[TOKEN_START_INDEX..];
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

        public async Task<RefreshToken> PopExistingTokenAsync(RefreshTokenDTO tokenDTO)
        {
            var token = await _refreshTokenRepository.GetByTokenStringAsync(tokenDTO.Token);
            if (token == null)
            {
                throw new TokenNotFoundException();
            }

            await _refreshTokenRepository.DeleteAsync(token);

            return token;
        }

        public bool IsValid(RefreshToken token)
        {
            var refreshJwtValidationParams = _refreshJwtConfig.ValidationParameters;
            refreshJwtValidationParams.IssuerSigningKey = GetKey(_refreshJwtConfig.GenerationParameters);

            try
            {
                new JwtSecurityTokenHandler().ValidateToken(
                    token.Token,
                    refreshJwtValidationParams,
                    out var securityToken
                );
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

        public Task InvalidateRefreshTokenOwnerAsync(RefreshToken token)
        {
            return _refreshTokenRepository.DeleteRelatedTokensByUserIdAsync(token.UserId);
        }

        public Task InvalidateAccessTokenOwnerAsync(string tokenString)
        {
            var userId = GetTokenOwnerId(tokenString);

            return _refreshTokenRepository.DeleteRelatedTokensByUserIdAsync(userId);
        }

        private Guid GetTokenOwnerId(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenString);

            var userIdString = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?
                .Value;

            if (userIdString == null)
            {
                throw new TokenClaimNotFoundException();
            }

            if (!Guid.TryParse(userIdString, out var userId))
            {
                throw new InvalidTokenClaimException();
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