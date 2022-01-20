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

        private async Task<bool> CheckIfExistAsync(UserRegistrationDTO userDTO)
        {
            return
                await _userRepository.GetByEmailAsync(userDTO.Email) != null ||
                await _userRepository.GetByUsernameAsync(userDTO.Username) != null;
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

        public async Task<User?> RegisterAsync(UserRegistrationDTO userDTO)
        {
            if (await CheckIfExistAsync(userDTO))
            {
                return null;
            }

            var user = _userMapper.Map<UserRegistrationDTO, User>(userDTO);

            return await _userRepository.CreateUserAsync(user);
        }        
    }
}