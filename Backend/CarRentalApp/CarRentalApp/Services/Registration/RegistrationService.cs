using CarRentalApp.Exceptions;
using CarRentalApp.Models.DTOs.Requests;
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

        /// <exception cref="GeneralException">User with such credentials already exists.</exception>
        public async Task<User> RegisterAsync(UserRegistrationDTO userDTO, bool isAdmin)
        {
            if (await _userService.CheckIfExistsAsync(userDTO))
            {
                throw new GeneralException(
                    ErrorTypes.Conflict,
                    "User already exists",
                    null
                );
            }

            return await _userService.RegisterAsync(userDTO, isAdmin);
        }
    }
}