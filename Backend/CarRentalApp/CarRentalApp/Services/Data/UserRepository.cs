using CarRentalApp.Models.Contexts;
using CarRentalApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Services.Data
{
    public class UserRepository
    {
        private readonly AuthenticationDbContext _authenticationDbContext;

        public UserRepository(AuthenticationDbContext authenticationDbContext)
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