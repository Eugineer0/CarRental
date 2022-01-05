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

        public async Task<User?> ValidateAndReturn(UserLoginDTO user2Login)
        {
            var existingUser = await _userRepository.GetByUsername(user2Login.Username);

            return 
                existingUser != null &&
                user2Login.Password.Equals(existingUser.Password) ?
                existingUser :
                null;
        }
    }
}
