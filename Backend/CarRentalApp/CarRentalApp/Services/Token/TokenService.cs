using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRentalApp.Configuration.JWT.Access;
using Microsoft.IdentityModel.Tokens;
using CarRentalApp.Models.Entities;
using CarRentalApp.Configuration.JWT;
using Microsoft.Extensions.Options;

namespace CarRentalApp.Services.Token
{
    public class TokenService
    {
        private readonly AccessJwtConfig _accessJwtConfig;

        public TokenService(IOptions<AccessJwtConfig> accessJwtConfig)
        {
            _accessJwtConfig = accessJwtConfig.Value;
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

        private SigningCredentials GetCredentials(GenerationParameters jwtConfig)
        {
            var key = Encoding.UTF8.GetBytes(jwtConfig.Secret);

            return new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );
        }
    }  
}