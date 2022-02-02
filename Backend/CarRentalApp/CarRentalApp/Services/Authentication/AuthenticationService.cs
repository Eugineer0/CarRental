using System.IdentityModel.Tokens.Jwt;
using CarRentalApp.Exceptions.BLL;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.DTOs.Responses;
using CarRentalApp.Models.Entities;
using CarRentalApp.Services.Identity;
using CarRentalApp.Services.Token;

namespace CarRentalApp.Services.Authentication
{
    public class AuthenticationService
    {
        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        public AuthenticationService(UserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(UserLoginDTO userDTO)
        {
            var user = await _userService.GetExistingUserAsync(userDTO);
            if (!_userService.IsValid(user, userDTO))
            {
                throw new InvalidUserException();
            }

            return await GetAccess(user);
        }

        public async Task<AuthenticationResponse> ReAuthenticateAsync(RefreshTokenDTO tokenDTO)
        {
            var token = await _tokenService.PopExistingTokenAsync(tokenDTO);
            if (!_tokenService.IsValid(token))
            {
                await _tokenService.InvalidateRefreshTokenOwnerAsync(token);
                throw new InvalidRefreshTokenException();
            }

            var user = await _userService.GetByRefreshTokenAsync(token);
            return await GetAccess(user);
        }

        public Task DeAuthenticateAsync(HttpRequest request)
        {
            var accessTokenString = _tokenService.GetTokenStringFromHeaders(request.Headers);
            return _tokenService.InvalidateAccessTokenOwnerAsync(accessTokenString);
        }

        public async Task<AuthenticationResponse> GetAccess(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

            var refreshToken = _tokenService.GenerateRefreshToken(user);
            var refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            await _tokenService.StoreRefreshTokenAsync(refreshTokenString, user);

            return new AuthenticationResponse()
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshTokenString,
            };
        }
    }
}