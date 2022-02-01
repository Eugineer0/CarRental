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

        public async Task<RegistrationResponse?> RegisterAsync(UserRegistrationDTO userDTO, bool isAdmin = false)
        {
            if (await _userService.CheckIfExistsAsync(userDTO))
            {
                return null;
            }

            var user = await _userService.RegisterAsync(userDTO, isAdmin);

            if (user == null)
            {
                return new RegistrationResponse()
                {
                    User = null,
                    StatusCode = 500,
                    Response = "Cannot insert into DB"
                };
            }

            return new RegistrationResponse()
            {
                User = user,
                StatusCode = 201,
                Response = "User registered successfully"
            };
        }
    }
}