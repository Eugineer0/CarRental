using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRentalApp.Configuration;
using Microsoft.IdentityModel.Tokens;
using CarRentalApp.Models.Data;

namespace CarRentalApp.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtSecurityToken GenerateAccessToken(User user)
        {
            var accessJwtGenerationParams = new TokenGenerationParameters();

            _configuration.GetSection("AccessJwtConfig:GenerationParameters").Bind(accessJwtGenerationParams);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.Role.ToString())
            };

            return Generate(accessJwtGenerationParams, claims);
        }
        
        private JwtSecurityToken Generate(TokenGenerationParameters jwtConfig, IEnumerable<Claim> jwtClaims)
        {
            var key = Encoding.UTF8.GetBytes(jwtConfig.Secret);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );

            var token = new JwtSecurityToken(
                issuer: jwtConfig.Issuer,
                audience: jwtConfig.Audience,
                claims: jwtClaims,
                expires: DateTime.Now.AddSeconds(jwtConfig.TokenLifeTimeSeconds),
                signingCredentials: credentials
            );

            return token;
        }
    }  
}