using System.IdentityModel.Tokens.Jwt;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.DTOs.Responces;
using CarRentalApp.Services.Identity;
using CarRentalApp.Services.Token;

namespace CarRentalApp.Services.Authentication
{

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResponse?> AuthenticateAsync(UserLoginDTO userDTO)
        {
            var user = await _userService.GetExistingUserAsync(userDTO);
            if (user == null || !_userService.Validate(user, userDTO))
            {
                return null;
            }

            var token = _tokenService.GenerateAccessToken(user);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthenticationResponse()
            { 
                AccessToken = tokenString
            };
        }
    }
}