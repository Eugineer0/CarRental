using CarRentalBll.Models;
using CarRentalBll.Services;
using CarRentalWeb.Models.Requests;
using CarRentalWeb.Models.Responses;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var model = request.Adapt<LoginModel>();
            var user = await _userService.GetValidClientAsync(model);
            return await AuthenticateAsync(user);
        }

        [HttpPost("login-admin")]
        public async Task<IActionResult> LoginAdmin(UserLoginRequest request)
        {
            var model = request.Adapt<LoginModel>();
            var user = await _userService.GetValidAdminAsync(model);
            return await AuthenticateAsync(user);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Refresh(RefreshAccessRequest request)
        {
            var token = await _tokenService.PopTokenAsync(request.RefreshToken);
            _tokenService.ValidateTokenLifetime(token.Token);
            var user = await _userService.GetByAsync(token.UserId);
            return await AuthenticateAsync(user);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout(RefreshAccessRequest request)
        {
            await _tokenService.PopTokenAsync(request.RefreshToken);
            return Ok();
        }

        [HttpPost("[action]")]
        public Task<IActionResult> Register(ClientRegistrationRequest request)
        {
            var model = request.Adapt<RegistrationModel>();
            return RegisterUserAsync(model);
        }

        [HttpPost("register-admin")]
        public Task<IActionResult> RegisterAdmin(AdminRegistrationRequest request)
        {
            var model = request.Adapt<RegistrationModel>();
            return RegisterUserAsync(model);
        }

        [Authorize]
        [HttpPost("complete-registration")]
        public async Task<IActionResult> CompleteRegistration(RegistrationCompletionRequest request)
        {
            var username = _tokenService.GetUsername(this.User.Claims);
            await _userService.AddDriverLicenseByAsync(username, request.DriverLicenseSerialNumber);
            return Ok();
        }

        private async Task<IActionResult> RegisterUserAsync(RegistrationModel model)
        {
            var userModel = await _userService.RegisterAsync(model);
            return Created($"api/users/{model.Username}", userModel);
        }

        private async Task<IActionResult> AuthenticateAsync(UserModel user)
        {
            var response = await _tokenService.GetAccessAsync(user);
            return Ok(response.Adapt<AuthenticationResponse>());
        }
    }
}