using CarRentalApp.Exceptions;
using CarRentalApp.Models.DTOs;
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

        /// <exception cref="SharedException">User with such credentials already exists.</exception>
        public async Task<User> RegisterAsync(UserRegistrationDTO userRegistrationDTO)
        {
            if (await _userService.CheckIfExistsAsync(userRegistrationDTO))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "User with such email or username already exists"
                );
            }

            return await _userService.RegisterAsync(userRegistrationDTO);
        }

        public async Task<User> CompleteRegistrationAsync(CompleteRegistrationDTO completeRegistrationDto)
        {
            var user = await _userService.GetExistingUserAsync(completeRegistrationDto);
            await _userService.MakeClientAsync(user, completeRegistrationDto);

            return user;
        }
    }
}