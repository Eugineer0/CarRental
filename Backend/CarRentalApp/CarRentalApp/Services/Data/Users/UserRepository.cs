using CarRentalApp.Contexts;
using CarRentalApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Services.Data
{
    public class UserRepository
    {
        private readonly CarRentalDbContext _authenticationDbContext;

        public UserRepository(CarRentalDbContext authenticationDbContext)
        {
            _authenticationDbContext = authenticationDbContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _authenticationDbContext.Users.Add(user);
            await _authenticationDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _authenticationDbContext.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _authenticationDbContext
                .Users
                .FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _authenticationDbContext
                .Users
                .FirstOrDefaultAsync(u => u.Username.Equals(username));
        }
    }
}