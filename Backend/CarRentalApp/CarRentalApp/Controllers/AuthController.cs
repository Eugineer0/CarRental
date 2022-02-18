using CarRentalApp.Exceptions;
using CarRentalApp.Models.DTOs;
using CarRentalApp.Models.DTOs.Registration;
using Microsoft.AspNetCore.Mvc;
using CarRentalApp.Services.Identity;
using CarRentalApp.Services.Token;

namespace CarRentalApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        public AuthController(
            UserService userService,
            TokenService tokenService
        )
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO model)
        {
            var user = await _userService.GetUserAsync(model.Username);
            if (_userService.Validate(user, model))
            {
                var authenticationResponse = await _tokenService.GetAccess(user);
                return Ok(authenticationResponse);
            }

            var token = _tokenService.GenerateRefreshToken(user);
            throw new SharedException(
                ErrorTypes.AdditionalDataRequired,
                token,
                "Registration completion required"
            );
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(UserLoginDTO model)
        {
            var user = await _userService.GetUserAsync(model.Username);
            _userService.ValidateAdmin(user, model);
            var authenticationResponse = await _tokenService.GetAccess(user);
            return Ok(authenticationResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(RefreshTokenDTO model)
        {
            var token = await _tokenService.PopTokenAsync(model);

            _tokenService.ValidateTokenLifetime(model);

            var user = await _userService.GetByRefreshTokenAsync(token);
            var authenticationResponse = await _tokenService.GetAccess(user);
            return Ok(authenticationResponse);
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout(RefreshTokenDTO model)
        {
            await _tokenService.PopTokenAsync(model);
            return Ok();
        }

        [HttpPost]
        public Task<IActionResult> RegisterClient(ClientRegistrationDTO model)
        {
            return Register(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDTO model)
        {
            await _userService.RegisterAsync(model);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CompleteRegistration(CompleteRegistrationDTO model)
        {
            var userId = _tokenService.GetUserId(model.Token);
            await _userService.CompleteRegistrationAsync(userId, model);
            return Ok();
        }
    }
}