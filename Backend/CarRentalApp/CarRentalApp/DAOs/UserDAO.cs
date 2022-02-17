using CarRentalApp.Contexts;
using CarRentalApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.DAOs
{
    public class UserDAO
    {
        private readonly CarRentalDbContext _carRentalDbContext;

        public UserDAO(CarRentalDbContext carRentalDbContext)
        {
            _carRentalDbContext = carRentalDbContext;
        }

        public Task InsertUserAsync(User user)
        {
            _carRentalDbContext.Users.Add(user);
            return _carRentalDbContext.SaveChangesAsync();
        }
        
        public Task<User?> GetByIdAsync(Guid id)
        {
            return _carRentalDbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            return _carRentalDbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Username.Equals(username));
        }

        public Task<bool> CheckUniquenessAsync(string username, string email)
        {
            return _carRentalDbContext.Users
                .AnyAsync(u => u.Username.Equals(username) || u.Email.Equals(email));
        }

        public Task UpdateUserAsync(User user)
        {
            _carRentalDbContext.Users.Update(user);
            return _carRentalDbContext.SaveChangesAsync();
        }
        
        public Task AssignRoleAsync(UserRole role)
        {
            _carRentalDbContext.UserRoles.Add(role);
            return _carRentalDbContext.SaveChangesAsync();
        }
        
        public Task AssignMultipleRolesAsync(List<UserRole> roles)
        {
            foreach (var role in roles)
            {
                _carRentalDbContext.UserRoles.Add(role);
            }
            
            return _carRentalDbContext.SaveChangesAsync();
        }
    }
}