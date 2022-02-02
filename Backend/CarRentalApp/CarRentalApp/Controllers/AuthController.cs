using System.Web.Http;
using CarRentalApp.Exceptions;
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
            catch (GeneralException exception)
            {
                switch (exception.ErrorType)
                {
                    case GeneralException.ErrorTypes.Invalid:
                        return Unauthorized(exception.Message);
                    case GeneralException.ErrorTypes.NotFound:
                        return Unauthorized(exception.Message);
                    case GeneralException.ErrorTypes.Conflict:
                        return Conflict(exception.Message);
                    default:
                        return new InternalServerErrorResult();
                }
            }

            return Ok(authenticationResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] TokenDTO model)
        {
            AuthenticationResponse authenticationResponse;

            try
            {
                authenticationResponse = await _authenticationService.ReAuthenticateAsync(model);
            }
            catch (GeneralException exception)
            {
                switch (exception.ErrorType)
                {
                    case GeneralException.ErrorTypes.Invalid:
                        return Unauthorized(exception.Message);
                    case GeneralException.ErrorTypes.NotFound:
                        return Unauthorized(exception.Message);
                    case GeneralException.ErrorTypes.Conflict:
                        return Conflict(exception.Message);
                    default:
                        return new InternalServerErrorResult();
                }
            }

            return Ok(authenticationResponse);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LogoutAll()
        {
            try
            {
                await _authenticationService.DeAuthenticateAllAsync(this.Request);
            }
            catch (GeneralException exception)
            {
                switch (exception.ErrorType)
                {
                    case GeneralException.ErrorTypes.Invalid:
                        return BadRequest(exception.Message);
                    case GeneralException.ErrorTypes.NotFound:
                        return BadRequest(exception.Message);
                    case GeneralException.ErrorTypes.Conflict:
                        return Conflict(exception.Message);
                    default:
                        return new InternalServerErrorResult();
                }
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
            catch (GeneralException exception)
            {
                switch (exception.ErrorType)
                {
                    case GeneralException.ErrorTypes.Invalid:
                        return BadRequest(exception.Message);
                    case GeneralException.ErrorTypes.NotFound:
                        return BadRequest(exception.Message);
                    case GeneralException.ErrorTypes.Conflict:
                        return Conflict(exception.Message);
                    default:
                        return new InternalServerErrorResult();
                }
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