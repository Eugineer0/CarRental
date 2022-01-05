using System.IdentityModel.Tokens.Jwt;
using CarRentalApp.Models.Data;

namespace CarRentalApp.Services.Token
{
    public interface ITokenService
    {
        JwtSecurityToken Generate(User user);
    }
}