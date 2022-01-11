using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.Data;

namespace CarRentalApp.Services.Identity
{
    public interface IUserService
    {
        Task<User?> GetExistingUserAsync(UserLoginDTO user);
        bool Validate(User user, UserLoginDTO userDTO);
    }
}