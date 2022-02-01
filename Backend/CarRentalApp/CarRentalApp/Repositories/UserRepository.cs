using CarRentalApp.Contexts;
using CarRentalApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Repositories
{
    public class UserRepository
    {
        private readonly CarRentalDbContext _carRentalDbContext;

        public UserRepository(CarRentalDbContext carRentalDbContext)
        {
            _carRentalDbContext = carRentalDbContext;
        }

        public Task InsertUserAsync(User user)
        {
            _carRentalDbContext.Users.Add(user);

            try
            {
                return _carRentalDbContext.SaveChangesAsync();
            }
            catch (NullReferenceException e)
            {
                throw;
            }
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            return _carRentalDbContext.Users.FindAsync(id).AsTask();
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

        public Task<bool> AreUniqueCredentialsAsync(string username, string email)
        {
             return _carRentalDbContext.Users
                .AnyAsync(u => u.Username.Equals(username) || u.Email.Equals(email));
        }
    }
}