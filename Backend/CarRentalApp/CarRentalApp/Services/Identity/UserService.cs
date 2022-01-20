using AutoMapper;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.Entities;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Services.Data;

namespace CarRentalApp.Services.Identity
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;

        private readonly IMapper _userMapper;

        public UserService(UserRepository userRepository, PasswordService passwordService, IMapper userMapper)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _userMapper = userMapper;
        }

        private async Task<bool> CheckIfExistAsync(UserRegistrationDTO user2Register)
        {
            return
                await _userRepository.GetByEmailAsync(user2Register.Email) != null ||
                await _userRepository.GetByUsernameAsync(user2Register.Username) != null;
        }

        public Task<User?> GetExistingUserAsync(UserLoginDTO user2Login)
        {
            return _userRepository.GetByUsernameAsync(user2Login.Username);
        }

        public bool ValidatePassword(User existingUser, UserLoginDTO user2Validate)
        {
            return _passwordService.VerifyPassword(
                existingUser.HashedPassword,
                existingUser.Salt,
                user2Validate.Password
            );
        }

        public Task<User?> GetByRefreshTokenAsync(RefreshToken token)
        {
            return _userRepository.GetByIdAsync(token.UserId);
        }

        public async Task<User?> RegisterAsync(UserRegistrationDTO user2Register)
        {
            if (await CheckIfExistAsync(user2Register))
            {
                return null;
            }

            var user = _userMapper.Map<UserRegistrationDTO, User>(user2Register);

            return await _userRepository.CreateUserAsync(user);
        }        
    }
}