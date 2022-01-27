using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Services.Registration;

namespace CarRentalApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;
        private readonly RegistrationService _registrationService;

        public AuthController(
            AuthenticationService authenticationService,
            RegistrationService registrationService
        )
        {
            _authenticationService = authenticationService;
            _registrationService = registrationService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            var authResponse = await _authenticationService.TryAuthenticateAsync(model);
            if (authResponse == null)
            {
                return Unauthorized("Incorrect username or password");
            }

            return Ok(authResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO model)
        {
            var authResponse = await _authenticationService.TryAuthenticateAsync(model);
            if (authResponse == null)
            {
                return Unauthorized("Invalid refresh token");
            }

            return Ok(authResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO model)
        {
            var user = await _registrationService.TryRegisterAsync(model);
            if (user == null)
            {
                return Conflict("User already exists");
            }

            var authResponse = await _authenticationService.GetAccess(user);

            return Ok(authResponse);
        }
    }
}