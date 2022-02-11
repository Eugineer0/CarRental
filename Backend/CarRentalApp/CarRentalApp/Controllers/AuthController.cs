using CarRentalApp.Models.DTOs;
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
        private readonly RoleService _roleService;

        public AuthController(
            AuthenticationService authenticationService,
            RegistrationService registrationService,
            RoleService roleService
        )
        {
            _authenticationService = authenticationService;
            _registrationService = registrationService;
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            var authenticationResponse = await _authenticationService.AuthenticateAsync(model);
            return Ok(authenticationResponse);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin([FromBody] UserLoginDTO model)
        {
            var authenticationResponse = await _authenticationService.AuthenticateAdminAsync(model);
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
        public async Task<IActionResult> Register([FromBody] ClientRegistrationDTO model)
        {
            var user = await _registrationService.RegisterAsync(model);
            var authenticationResponse = await _authenticationService.GetAccess(user);
            return Ok(authenticationResponse);
        }
        
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegistrationDTO model)
        {
            var user = await _registrationService.RegisterAsync(model);
            var authenticationResponse = await _authenticationService.GetAccess(user);
            return Ok(authenticationResponse);
        }
        
        [HttpPost]
        public async Task<IActionResult> ProceedRegistration([FromBody] UserRegistrationDTO model)
        {
            var user = await _registrationService.RegisterAsync(model);
            var authenticationResponse = await _authenticationService.GetAccess(user);
            return Ok(authenticationResponse);
        }
    }
}