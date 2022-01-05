using CarRentalApp.Models.Data;

namespace CarRentalApp.Services.Data
{
    public class UserRepositoryService : IUserRepository
    {
        public Task<User> CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}