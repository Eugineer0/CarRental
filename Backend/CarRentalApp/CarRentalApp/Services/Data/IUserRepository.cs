using CarRentalApp.Models.Data;

namespace CarRentalApp.Services.Data
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);

        Task<User> CreateUserAsync(User user);
    }
}