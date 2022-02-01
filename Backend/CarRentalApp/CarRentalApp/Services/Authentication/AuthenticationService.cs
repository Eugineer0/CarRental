﻿using System.IdentityModel.Tokens.Jwt;
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

        public async Task<AuthenticationResponse?> AuthenticateAsync(UserLoginDTO userDTO)
        {
            var user = await _userService.GetExistingUserAsync(userDTO);
            if (user == null || !_userService.ValidatePassword(user, userDTO))
            {
                return null;
            }

            return await GetAccess(user);
        }

        public async Task<AuthenticationResponse?> ReAuthenticateAsync(RefreshTokenDTO tokenDTO)
        {
            var token = await _tokenService.PopExistingTokenAsync(tokenDTO);
            if (token == null)
            {
                return null;
            }

            if (!_tokenService.Validate(token))
            {
                await _tokenService.InvalidateUserByIdAsync(token.UserId);
            }

            var user = await _userService.GetByRefreshTokenAsync(token);
            if (user == null)
            {
                return null;
            }

            return await GetAccess(user);
        }
        
        public Task<bool> DeAuthenticateAsync(HttpRequest request)
        {
            var accessToken = _tokenService.GetTokenFromHeaders(request.Headers);
            
            var userId = _tokenService.GetUserIdByToken(accessToken);
            if (userId.HasValue)
            {
                return _tokenService.InvalidateUserByIdAsync(userId.Value);
            }

            return Task.FromResult(false);
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
                RefreshToken = refreshTokenString
            };
        }
    }
}