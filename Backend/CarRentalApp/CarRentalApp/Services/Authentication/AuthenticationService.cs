using CarRentalApp.Exceptions;
using CarRentalApp.Models.DTOs;
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
        
        public async Task<AuthenticationDTO> AuthenticateAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _userService.GetExistingUserAsync(userLoginDTO);
            if (_userService.ValidateClient(user, userLoginDTO))
            {
                return await GetAccess(user);
            }
            
            var token = _tokenService.GenerateRefreshToken(user);
            throw new SharedException(
                ErrorTypes.AdditionalDataRequired,
                token,
                "DriverLicenseSerialNumber field required"
            );
        }

        public async Task<AuthenticationDTO> AuthenticateAdminAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _userService.GetExistingUserAsync(userLoginDTO);
            _userService.ValidateAdmin(user, userLoginDTO);

            return await GetAccess(user);
        }

        public async Task<AuthenticationDTO> ReAuthenticateAsync(RefreshTokenDTO refreshTokenDTO)
        {
            var token = await _tokenService.PopTokenAsync(refreshTokenDTO);

            _tokenService.ValidateTokenLifetime(refreshTokenDTO);

            var user = await _userService.GetUserByRefreshTokenAsync(token);
            return await GetAccess(user);
        }

        public async Task DeAuthenticateAsync(RefreshTokenDTO refreshTokenDTO)
        {
            await _tokenService.PopTokenAsync(refreshTokenDTO);
        }

        public async Task<AuthenticationDTO> GetAccess(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken(user);

            await _tokenService.StoreRefreshTokenAsync(refreshToken, user);

            return new AuthenticationDTO()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}