using CarRentalApp.Models.Data;
using CarRentalApp.Services.Data;

namespace WebApplicationTest.Services.Data
{
    public class UserRepositoryService : IUserRepository
    {
        public async Task<User> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }        

        public async Task<User?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }
    }
}
