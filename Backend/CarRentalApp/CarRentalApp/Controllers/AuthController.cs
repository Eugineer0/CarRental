using CarRentalApp.Exceptions.BLL;
using CarRentalApp.Exceptions.DAL;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.DTOs.Responses;
using CarRentalApp.Models.Entities;
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
            AuthenticationResponse authenticationResponse;

            try
            {
                authenticationResponse = await _authenticationService.AuthenticateAsync(model);
            }
            catch (InvalidRefreshTokenException)
            {
                return Unauthorized("Invalid refresh token");
            }
            
            return Ok(authenticationResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO model)
        {
            AuthenticationResponse authenticationResponse;
            
            try
            {
                authenticationResponse = await _authenticationService.ReAuthenticateAsync(model);
            }
            catch (InvalidRefreshTokenException)
            {
                return Unauthorized("Invalid refresh token");
            }

            return Ok(authenticationResponse);
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authenticationService.DeAuthenticateAsync(this.Request);
            }
            catch (InvalidTokenPayloadException)
            {
                return BadRequest("Invalid token payload");
            }
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO model, bool isAdmin = false)
        {
            User user;
            
            try
            {
                user = await _registrationService.RegisterAsync(model, isAdmin);
            }
            catch (UserAlreadyExistsException)
            {
                return Conflict("User already exists");
            }

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