using CarRentalApp.Contexts;
using CarRentalApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Services.Data.Users
{
    public class UserRepository
    {
        private readonly CarRentalDbContext _carRentalDbContext;

        public UserRepository(CarRentalDbContext carRentalDbContext)
        {
            _carRentalDbContext = carRentalDbContext;
        }

        public async Task<User?> InsertUserAsync(User user)
        {
            _carRentalDbContext.Users.Add(user);

            try
            {
                await _carRentalDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return null;
            }
            
            return user;
        }

        public ValueTask<User?> GetByIdAsync(Guid id)
        {
            return _carRentalDbContext.Users.FindAsync(id);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return _carRentalDbContext.Users
                .FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            return _carRentalDbContext.Users
                .FirstOrDefaultAsync(u => u.Username.Equals(username));
        }

        public Task<bool> IsUniqueCredentialsAsync(string username, string email)
        {
             return _carRentalDbContext.Users
                .AnyAsync(u => u.Username.Equals(username) || u.Email.Equals(email));
        }
    }
}