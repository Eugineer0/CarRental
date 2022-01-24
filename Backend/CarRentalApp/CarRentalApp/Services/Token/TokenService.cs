using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CarRentalApp.Configuration.JWT.Access;
using Microsoft.IdentityModel.Tokens;
using CarRentalApp.Models.Entities;
using CarRentalApp.Configuration.JWT;
using Microsoft.Extensions.Options;
using CarRentalApp.Services.Data;
using CarRentalApp.Configuration.JWT.Refresh;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Services.Data.Tokens;

namespace CarRentalApp.Services.Token
{
    public class TokenService
    {
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
            return new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtParams.Secret)
            );
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
                expires: DateTime.Now.AddSeconds(accessJwtGenerationParams.LifeTimeSeconds),
                signingCredentials: GetCredentials(accessJwtGenerationParams)
            );
        }

        public JwtSecurityToken GenerateRefreshToken(User user)
        {
            var refreshJwtGenerationParams = _refreshJwtConfig.GenerationParameters;

            var jwtClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            };

            return new JwtSecurityToken(
                claims: jwtClaims,
                expires: DateTime.Now.AddSeconds(refreshJwtGenerationParams.LifeTimeSeconds),
                signingCredentials: GetCredentials(refreshJwtGenerationParams)
            );
        }

        public async Task<RefreshToken?> GetExistingTokenAsync(RefreshTokenDTO tokenDTO)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(tokenDTO.Token);
            if (token == null)
            {
                await InvalidateUserAsync(tokenDTO);
                return null;
            }

            await _refreshTokenRepository.DeleteAsync(token);

            return token;
        }

        public bool Validate(RefreshToken token)
        {
            var refreshJwtValidationParams = _refreshJwtConfig.ValidationParameters;

            refreshJwtValidationParams.IssuerSigningKey
                = GetKey(_refreshJwtConfig.GenerationParameters);

            try
            {
                new JwtSecurityTokenHandler()
                    .ValidateToken(
                        token.Token,
                        refreshJwtValidationParams,
                        out SecurityToken securityToken
                    );
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<RefreshToken> StoreRefreshTokenAsync(
            string refreshTokenString,
            User user
        )
        {
            var refreshToken = new RefreshToken()
            {
                Token = refreshTokenString,
                UserId = user.Id
            };

            return await _refreshTokenRepository.CreateAsync(refreshToken);
        }

        private Task InvalidateUserAsync(RefreshTokenDTO tokenDTO)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenDTO.Token);

            var userIdString = jwtSecurityToken.Claims
                .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sid)?.Value;

            if (Guid.TryParse(userIdString, out Guid userId))
            {
                return _refreshTokenRepository.DeleteRelatedTokensAsync(userId);
            }

            return Task.CompletedTask;
        }

        private SigningCredentials GetCredentials(GenerationParameters jwtParams)
        {
            return new SigningCredentials(
                GetKey(jwtParams),
                SecurityAlgorithms.HmacSha256Signature
            );
        }
    }
}