using CarRentalApp.Models.Data;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Services.Data;

namespace CarRentalApp.Services.Identity
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetExistingUserAsync(UserLoginDTO user2Login)
        {            
            return await _userRepository.GetByUsernameAsync(user2Login.Username);
        }

        public bool Validate(User existingUser, UserLoginDTO userDTO)
        {
            var hashedPassword = DigestPassword(userDTO.Password, existingUser.Salt);
            return hashedPassword.Equals(existingUser.HashedPassword);
        }

        private string DigestPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + salt);
        }
    }
}