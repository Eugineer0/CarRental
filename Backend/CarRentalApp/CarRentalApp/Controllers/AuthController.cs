using CarRentalApp.Models.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Services.Registration;
using Microsoft.AspNetCore.Authorization;

namespace CarRentalApp.Controllers
{
    [ApiController]
    [Route("api/[action]")]
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
            var authenticationResponse = await _authenticationService.AuthenticateAsync(model);
            return Ok(authenticationResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO model)
        {
            var authenticationResponse = await _authenticationService.ReAuthenticateAsync(model);
            return Ok(authenticationResponse);
        }

        
        [HttpPost]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDTO model)
        {
            await _authenticationService.DeAuthenticateAsync(model);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO model, bool isAdmin = false)
        {
            var user = await _registrationService.RegisterAsync(model, isAdmin);
            var authenticationResponse = await _authenticationService.GetAccess(user);
            return Ok(authenticationResponse);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public Task<IActionResult> RegisterAdmin([FromBody] UserRegistrationDTO model)
        {
            return Register(model, true);
        }
    }
}