using CarRentalApp.Models.Entities;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Services.Identity;

namespace CarRentalApp.Services.Registration
{
    
    public class RegistrationService
    {
        private readonly UserService _userService;

        public RegistrationService(UserService userService)
        {
            _userService = userService;
        }        

        public async Task<User?> RegisterAsync(UserRegistrationDTO userDTO)
        {           
            return await _userService.RegisterAsync(userDTO);
        }
    }
}