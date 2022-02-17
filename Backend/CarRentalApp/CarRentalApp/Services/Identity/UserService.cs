﻿using AutoMapper;
using CarRentalApp.Configuration;
using CarRentalApp.DAOs;
using CarRentalApp.Exceptions;
using CarRentalApp.Models;
using CarRentalApp.Models.DTOs;
using CarRentalApp.Models.Entities;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Services.Token;
using Microsoft.Extensions.Options;

namespace CarRentalApp.Services.Identity
{
    public class UserService
    {
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;

        private readonly IMapper _userMapper;

        private readonly UserDAO _userDAO;

        private readonly ClientRequirements _clientRequirements;

        public UserService(
            UserDAO userDAO,
            PasswordService passwordService,
            IMapper userMapper,
            IOptions<ClientRequirements> clientRequirements, TokenService tokenService)
        {
            _userDAO = userDAO;
            _passwordService = passwordService;
            _userMapper = userMapper;
            _tokenService = tokenService;
            _clientRequirements = clientRequirements.Value;
        }

        public Task<bool> CheckIfExistsAsync(UserRegistrationDTO userRegistrationDTO)
        {
            return _userDAO.CheckUniquenessAsync(userRegistrationDTO.Username, userRegistrationDTO.Email);
        }

        /// <exception cref="SharedException">User not found by <paramref name="userInfo"/> username.</exception>
        public async Task<User> GetExistingUserAsync(IContainUniqueUsername userInfo)
        {
            var user = await _userDAO.GetByUsernameAsync(userInfo.Username);
            if (user == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "User not found",
                    "User with such username not found"
                );
            }

            return user;
        }

        /// <exception cref="SharedException">
        /// User not found by <paramref name="completeRegistrationDTO"/> token.</exception>
        public async Task<User> GetExistingUserAsync(CompleteRegistrationDTO completeRegistrationDTO)
        {
            var userId = _tokenService.GetUserId(completeRegistrationDTO.Token);
            var user = await _userDAO.GetByIdAsync(userId);
            if (user == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "User not found",
                    "User with such username not found"
                );
            }

            return user;
        }

        /// <exception cref="SharedException"><paramref name="token"/> subject not found.</exception>
        public async Task<User> GetUserByRefreshTokenAsync(RefreshToken token)
        {
            var user = await _userDAO.GetByIdAsync(token.UserId);
            if (user == null)
            {
                throw new SharedException(
                    ErrorTypes.AuthFailed,
                    "Invalid token",
                    "User not found by refresh token userId"
                );
            }

            return user;
        }

        public async Task<User> RegisterAsync(UserRegistrationDTO clientRegistrationDTO)
        {
            var user = CreateFromDTO(clientRegistrationDTO);

            await _userDAO.InsertUserAsync(user);

            return user;
        }

        public Task MakeClientAsync(User user, CompleteRegistrationDTO completeRegistrationDto)
        {
            user.DriverLicenseSerialNumber = completeRegistrationDto.DriverLicenseSerialNumber;
            user.Roles.Add(new UserRole() {Role = Roles.Client});

            return _userDAO.UpdateUserAsync(user);
        }

        public Task UpdateRolesAsync(User user, UserDTO userDTO)
        {
            if (user.Roles.Select(role => role.Role).SequenceEqual(userDTO.Roles))
            {
                return Task.CompletedTask;
            }

            user.Roles = userDTO.Roles.Select(role => new UserRole() {Role = role}).ToList();

            return _userDAO.UpdateUserAsync(user);
        }

        private User CreateFromDTO(UserRegistrationDTO userRegistrationDTO)
        {
            var user = _userMapper.Map<UserRegistrationDTO, User>(userRegistrationDTO);

            user.Salt = _passwordService.GenerateSalt();
            user.HashedPassword = _passwordService.DigestPassword(userRegistrationDTO.Password, user.Salt);

            user.Roles = new List<UserRole>() {new UserRole() {Role = Roles.None}};

            return user;
        }
        
        /// <exception cref="SharedException">Additional user info required.</exception>
        /// <exception cref="SharedException">Client role is not approved.</exception>
        public bool ValidateClient(User user, UserLoginDTO userLoginDTO)
        {
            ValidatePassword(user.HashedPassword, user.Salt, userLoginDTO.Password);

            if (user.Roles.Select(role => role.Role).Intersect(UserRole.ClientRoles).Any())
            {
                return true;
            }

            if (user.DriverLicenseSerialNumber == null)
            {
                return false;
            }

            throw new SharedException(
                ErrorTypes.AccessDenied,
                "Wait for your account to be approved",
                "User does not have client role"
            );
        }

        /// <exception cref="SharedException">Inconsistent user role.</exception>
        public void ValidateAdmin(User user, UserLoginDTO userLoginDTO)
        {
            ValidatePassword(user.HashedPassword, user.Salt, userLoginDTO.Password);

            if (!user.Roles.Select(role => role.Role).Intersect(UserRole.AdminRoles).Any())
            {
                throw new SharedException(
                    ErrorTypes.AccessDenied,
                    "You do not have permission",
                    "User does not have admin role"
                );
            }
        }

        /// <exception cref="SharedException">Empty roles set.</exception>
        /// <exception cref="SharedException">Cannot specify client role without additional info.</exception>
        public void ValidateNewRoles(User user, UserDTO userDTO)
        {
            if (userDTO.Roles.Count < 1)
            {
                throw new SharedException(
                    ErrorTypes.Invalid,
                    "Role changing failed",
                    "User must have at least 1 role"
                );
            }

            if (userDTO.Roles.Intersect(UserRole.ClientRoles).Any())
            {
                ValidateAge(user.DateOfBirth);

                if (user.DriverLicenseSerialNumber == null)
                {
                    throw new SharedException(
                        ErrorTypes.Conflict,
                        "Role changing failed",
                        "User cannot become a client without specifying DriverLicenseSerialNumber"
                    );
                }
            }
        }

        /// <exception cref="SharedException">Invalid user age.</exception>
        private void ValidateAge(DateTime dateOfBirth)
        {
            var minimumAge = _clientRequirements.MinimumAge;
            var criticalDate = dateOfBirth.AddYears(minimumAge);

            if (DateTime.Now.CompareTo(criticalDate) > 0)
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Client`s data does not meet requirements",
                    $"Client`s age must be greater than {minimumAge - 1}"
                );
            }
        }

        /// <exception cref="SharedException">Incorrect username or password.</exception>
        private void ValidatePassword(byte[] hashedPassword, byte[] salt, string password)
        {
            if (_passwordService.VerifyPassword(hashedPassword, salt, password))
            {
                return;
            }

            throw new SharedException(
                ErrorTypes.AuthFailed,
                "Incorrect username or password",
                "Incorrect password"
            );
        }
    }
}