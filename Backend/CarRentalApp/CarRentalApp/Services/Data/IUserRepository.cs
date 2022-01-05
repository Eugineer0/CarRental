using CarRentalApp.Models.Data;

namespace CarRentalApp.Services.Data
{
    public interface IUserRepository
    {
        Task<User> GetByEmail (string email);
        Task<User> GetByUsername (string username); 
        Task<User> CreateUser (User user);
    }
}