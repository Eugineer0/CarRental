using CarRentalApp.Models.BLL;
using CarRentalApp.Models.WEB.Requests;
using CarRentalApp.Services;
using Mapster;
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
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var model = request.Adapt<LoginModel>();
            var response = await _userService.LoginAsync(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(UserLoginRequest request)
        {
            var model = request.Adapt<LoginModel>();
            var response = await _userService.LoginAdminAsync(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(RefreshAccessRequest request)
        {
            var response = await _userService.RefreshAccessAsync(request.RefreshToken);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(RefreshAccessRequest request)
        {
            await _tokenService.PopTokenAsync(request.RefreshToken);
            return Ok();
        }

        [HttpPost]
        public Task<IActionResult> Register(ClientRegistrationRequest request)
        {
            var model = request.Adapt<RegistrationModel>();
            return RegisterModel(model);
        }

        [HttpPost]
        public Task<IActionResult> RegisterAdmin(AdminRegistrationRequest request)
        {
            var model = request.Adapt<RegistrationModel>();
            return RegisterModel(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CompleteRegistration(RegistrationCompletionRequest request)
        {
            await _userService.CompleteRegistrationAsync(request.DriverLicenseSerialNumber, request.Token);
            return Ok();
        }

        private async Task<IActionResult> RegisterModel(RegistrationModel model)
        {
            await _userService.RegisterAsync(model);
            return Created($"api/users/{model.Username}", null);
        }
    }
}