using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRentalApp.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CarRentalApp.Models.Data;

namespace CarRentalApp.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _appSettings;

        public TokenService(IOptions<JwtConfig> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        
        public JwtSecurityToken Generate(User user)
        {                       
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim("role", user.Role.ToString())
            };                           

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            );

            var token = new JwtSecurityToken(
                issuer: _appSettings.Issuer,
                audience: _appSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddSeconds(_appSettings.TokenLifeTimeSeconds),
                signingCredentials: credentials
            );
            
            return token;
        }        
    }
}