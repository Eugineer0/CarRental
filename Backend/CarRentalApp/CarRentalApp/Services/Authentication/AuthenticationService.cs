using System.IdentityModel.Tokens.Jwt;
using CarRentalApp.Exceptions;
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

        /// <exception cref="GeneralException">Incorrect username or password.</exception>
        public async Task<AuthenticationResponse> AuthenticateAsync(UserLoginDTO userDTO)
        {
            var user = await _userService.GetExistingUserAsync(userDTO);
            if (!_userService.CheckIfValid(user, userDTO))
            {
                throw new GeneralException(
                    ErrorTypes.AuthFailed,
                    "Incorrect username or password",
                    null
                );
            }

            return await GetAccess(user);
        }

        /// <exception cref="GeneralException"><paramref name="refreshTokenDto"/> is invalid.</exception>
        public async Task<AuthenticationResponse> ReAuthenticateAsync(RefreshTokenDTO refreshTokenDto)
        {
            if (!await _tokenService.CheckIfValidAsync(refreshTokenDto))
            {
                throw new GeneralException(
                    ErrorTypes.AuthFailed,
                    "Invalid refresh token",
                    null
                );
            }

            var token = await _tokenService.PopTokenAsync(refreshTokenDto);

            var user = await _userService.GetUserByRefreshTokenAsync(token);
            return await GetAccess(user);
        }

        public async Task DeAuthenticateAsync(RefreshTokenDTO refreshTokenDto)
        {
            if (!await _tokenService.CheckIfValidAsync(refreshTokenDto))
            {
                throw new GeneralException(
                    ErrorTypes.AuthFailed,
                    "Invalid refresh token",
                    null
                );
            }
            
            await _tokenService.InvalidateRefreshTokenAsync(refreshTokenDto);
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