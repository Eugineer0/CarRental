using CarRentalApp.Models.Data;

namespace CarRentalApp.Services.Data
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByUsernameAsync(string username);

        Task<User> CreateUserAsync(User user);
    }
}