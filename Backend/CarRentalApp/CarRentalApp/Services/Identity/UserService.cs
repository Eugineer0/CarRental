using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.Entities;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Services.Data;

namespace CarRentalApp.Services.Identity
{
    public class UserService
    {
        private UserRepository _userRepository;
        private PasswordService _passwordService;

        public UserService(UserRepository userRepository, PasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<User?> GetExistingUserAsync(UserLoginDTO user2Login)
        {
            return await _userRepository.GetByUsernameAsync(user2Login.Username);
        }

        public bool Validate(User existingUser, UserLoginDTO user2Validate)
        {
            return _passwordService.VerifyPassword(
                existingUser.HashedPassword,
                existingUser.Salt,
                user2Validate.Password
            );
        }        
    }
}