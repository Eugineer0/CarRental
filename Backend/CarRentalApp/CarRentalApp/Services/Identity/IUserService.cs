using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.Data;

namespace CarRentalApp.Services.Identity
{
    public interface IUserService
    {
        Task<User?> GetValidUserAsync(UserLoginDTO user);
    }
}