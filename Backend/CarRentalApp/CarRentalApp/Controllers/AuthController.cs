using CarRentalApp.Models.Dto;
using CarRentalApp.Models.Dto.Registration;
using CarRentalApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            var access = await _userService.LoginAsync(model);
            return Ok(access);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(UserLoginDto model)
        {
            var access = await _userService.LoginAdminAsync(model);
            return Ok(access);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(RefreshTokenDto model)
        {
            var access = await _userService.RefreshAccessAsync(model);
            return Ok(access);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(RefreshTokenDto model)
        {
            await _tokenService.PopTokenAsync(model);
            return Ok();
        }

        [HttpPost]
        public Task<IActionResult> RegisterClient(ClientRegistrationDto model)
        {
            return Register(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDto model)
        {
            await _userService.RegisterAsync(model);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CompleteRegistration(CompleteRegistrationDto model)
        {
            await _userService.CompleteRegistrationAsync(model);
            return Ok();
        }
    }
}