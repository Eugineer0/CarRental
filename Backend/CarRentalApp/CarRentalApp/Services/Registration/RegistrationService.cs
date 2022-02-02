using CarRentalApp.Exceptions.DAL;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.DTOs.Responses;
using CarRentalApp.Models.Entities;
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

        public async Task<User> RegisterAsync(UserRegistrationDTO userDTO, bool isAdmin)
        {
            if (await _userService.CheckIfExistsAsync(userDTO))
            {
                throw new UserAlreadyExistsException();
            }

            var user = await _userService.RegisterAsync(userDTO, isAdmin);

            return user;
        }
    }
}