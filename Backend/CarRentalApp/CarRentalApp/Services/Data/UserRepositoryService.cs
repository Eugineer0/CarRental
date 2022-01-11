using CarRentalApp.Models.Contexts;
using CarRentalApp.Models.Data;
using CarRentalApp.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationTest.Services.Data
{
    public class UserRepositoryService : IUserRepository
    {
        private readonly AuthenticationDbContext _authenticationDbContext;

        public UserRepositoryService(AuthenticationDbContext authenticationDbContext)
        {
            _authenticationDbContext = authenticationDbContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _authenticationDbContext.Users.Add(user);
            await _authenticationDbContext.SaveChangesAsync();

            return user;
        }        

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _authenticationDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
