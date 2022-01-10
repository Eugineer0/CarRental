using CarRentalApp.Models.Data;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Services.Data;
using System.Security.Cryptography;
using System.Text;

namespace CarRentalApp.Services.Identity
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetValidUserAsync(UserLoginDTO user2Login)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(user2Login.Username);

            return
                existingUser != null &&
                Validate(user2Login, existingUser) ?
                existingUser :
                null;
        }

        private bool Validate(UserLoginDTO userDTO, User existingUser)
        {
            var hashedPassword = DigestPassword(userDTO.Password, existingUser.Salt);
            return hashedPassword.SequenceEqual(existingUser.HashedPassword);
        }

        private byte[] DigestPassword(string password, string salt)
        {
            byte[] data = Encoding.UTF8.GetBytes(password + salt);

            HashAlgorithm sha = SHA256.Create();

            return sha.ComputeHash(data);
        }
    }
}