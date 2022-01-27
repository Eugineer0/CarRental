using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Models.Requests.DTOs;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Services.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;

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
            if (authenticationResponse == null)
            {
                return Unauthorized("Incorrect username or password");
            }

            return Ok(authenticationResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO model)
        {
            var authenticationResponse = await _authenticationService.ReAuthenticateAsync(model);
            if (authenticationResponse == null)
            {
                return Unauthorized("Invalid refresh token");
            }

            return Ok(authenticationResponse);
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var authorizationToken = Request.Headers[HeaderNames.Authorization].ToString()[7..];

            if (await _authenticationService.DeAuthenticateAsync(authorizationToken))
            {
                return Ok();
            }

            return BadRequest("Invalid token payload");
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO model)
        {
            var registrationResponse = await _registrationService.RegisterAsync(model);
            if (registrationResponse == null)
            {
                return Conflict("User already exists");
            }

            if (registrationResponse.User == null)
            {
                return Problem(
                    statusCode: registrationResponse.StatusCode,
                    detail: registrationResponse.Response
                );
            }

            var authenticationResponse = await _authenticationService.GetAccess(registrationResponse.User);

            return Ok(authenticationResponse);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegistrationDTO model)
        {
            var registrationResponse = await _registrationService.RegisterAsync(model, true);
            if (registrationResponse == null)
            {
                return Conflict("Admin already exists");
            }

            if (registrationResponse.User == null)
            {
                return Problem(
                    statusCode: registrationResponse.StatusCode,
                    detail: registrationResponse.Response
                );
            }

            var authenticationResponse = await _authenticationService.GetAccess(registrationResponse.User);

            return Ok(authenticationResponse);
        }
    }
}