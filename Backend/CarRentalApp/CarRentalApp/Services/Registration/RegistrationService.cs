using System.Threading.Tasks;
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

        public async Task<User?> TryRegisterAsync(UserRegistrationDTO userDTO)
        {
            if (await _userService.CheckIfExistsAsync(userDTO))
            {
                return null;
            }

            return await _userService.RegisterAsync(userDTO);
        }
    }
}