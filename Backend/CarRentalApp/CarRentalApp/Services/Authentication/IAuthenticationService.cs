using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.DTOs.Responces;

namespace CarRentalApp.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse?> AuthenticateAsync(UserLoginDTO userDTO);
    }
}