﻿using AutoMapper;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.Entities;
using CarRentalApp.Repositories;
using CarRentalApp.Services.Authentication;

namespace CarRentalApp.Services.Identity
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;

        private readonly IMapper _userMapper;

        public UserService(
            UserRepository userRepository,
            PasswordService passwordService,
            IMapper userMapper
        )
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _userMapper = userMapper;
        }

        public Task<User?> GetExistingUserAsync(UserLoginDTO userDTO)
        {
            return _userRepository.GetByUsernameAsync(userDTO.Username);
        }

        public bool ValidatePassword(User existingUser, UserLoginDTO userDTO)
        {
            return _passwordService.VerifyPassword(
                existingUser.HashedPassword,
                existingUser.Salt,
                userDTO.Password
            );
        }

        public Task<User?> GetByRefreshTokenAsync(RefreshToken token)
        {
            return _userRepository.GetByIdAsync(token.UserId);
        }

        public async Task<User?> RegisterAsync(UserRegistrationDTO userDTO, bool isAdmin)
        {
            var user = CreateFromDTO(userDTO, isAdmin);

            await _userRepository.InsertUserAsync(user);

            return user;
        }

        public Task<bool> CheckIfExistsAsync(UserRegistrationDTO userDTO)
        {
            return _userRepository.AreUniqueCredentialsAsync(
                userDTO.Username, userDTO.Email
            );
        }

        private User CreateFromDTO(UserRegistrationDTO userDTO, bool isAdmin)
        {
            var user = _userMapper.Map<UserRegistrationDTO, User>(userDTO);
            
            user.Salt = _passwordService.GenerateSalt();
            user.HashedPassword = _passwordService.DigestPassword(userDTO.Password, user.Salt);
            user.Role = isAdmin ? Role.Admin : Role.User;

            return user;
        }
    }
}