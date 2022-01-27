using System.Threading.Tasks;
using AutoMapper;
using CarRentalApp.Models.Entities;
using CarRentalApp.Models.Requests.DTOs;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Services.Data.Users;
using Microsoft.AspNetCore.Authorization.Infrastructure;

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

        public ValueTask<User?> GetByRefreshTokenAsync(RefreshToken token)
        {
            return _userRepository.GetByIdAsync(token.UserId);
        }

        public async Task<User?> RegisterAsync(UserRegistrationDTO userDTO, bool isAdmin)
        {
            var user = CreateFromDTO(userDTO, isAdmin);

            return await _userRepository.InsertUserAsync(user);
        }

        public Task<bool> CheckIfExistsAsync(UserRegistrationDTO userDTO)
        {
            return _userRepository.IsUniqueCredentialsAsync(
                userDTO.Username, userDTO.Email
            );
        }

        private User CreateFromDTO(UserRegistrationDTO userDTO, bool isAdmin)
        {
            var user = _userMapper.Map<UserRegistrationDTO, User>(userDTO);
            var role = isAdmin ? Role.Admin : Role.User;

            return user;
        }
    }
}