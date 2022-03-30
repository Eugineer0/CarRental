using System.Security.Claims;
using CarRentalBll.Models;
using CarRentalBll.Services;
using CarRentalWeb.Models.Requests;
using CarRentalWeb.Models.Responses;
using CarRentalWeb.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedResources.Exceptions;

namespace CarRentalWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TokenService _tokenService;
        private readonly JwtService _jwtService;

        public AuthController(
            UserService userService,
            TokenService tokenService,
            JwtService jwtService
        )
        {
            _userService = userService;
            _tokenService = tokenService;
            _jwtService = jwtService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var model = request.Adapt<LoginModel>();
            var possiblyClient = await _userService.ValidateAsClientAsync(model);
            switch (possiblyClient.Status)
            {
                case UserStatus.Ok:
                {
                    return await AuthenticateAsync(possiblyClient.User);
                }
                case UserStatus.NotEnoughInfo:
                {
                    var token = _jwtService.GenerateToken(possiblyClient.User.Id);
                    throw new SharedException(
                        ErrorTypes.AdditionalDataRequired,
                        token,
                        "Registration completion required"
                    );
                }
                case UserStatus.Unapproved:
                {
                    throw new SharedException(
                        ErrorTypes.AccessDenied,
                        "Wait for your account to be approved",
                        "User does not have client role"
                    );
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost("login-admin")]
        public async Task<IActionResult> LoginAdmin(UserLoginRequest request)
        {
            var model = request.Adapt<LoginModel>();
            var possiblyAdmin = await _userService.ValidateAsAdminAsync(model);
            switch (possiblyAdmin.Status)
            {
                case UserStatus.Ok:
                {
                    return await AuthenticateAsync(possiblyAdmin.User);
                }
                case UserStatus.Unapproved:
                {
                    throw new SharedException(
                        ErrorTypes.AccessDenied,
                        "You do not have permission",
                        "User does not have admin role"
                    );
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Refresh(RefreshAccessRequest request)
        {
            var token = await _tokenService.PopTokenAsync(request.RefreshToken);
            if (token == null)
            {
                var userId = _jwtService.GetUserId(request.RefreshToken);
                await _tokenService.RevokeTokensByAsync(userId);

                throw new SharedException(
                    ErrorTypes.AuthFailed,
                    "Invalid refresh token",
                    "Refresh token not found, all related sessions will be closed"
                );
            }

            _jwtService.ValidateTokenLifetime(token.Token);
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
            var userId = GetUserId(this.User.Claims);
            await _userService.AddDriverLicenseByAsync(userId, request.DriverLicenseSerialNumber);
            return Ok();
        }

        private async Task<IActionResult> RegisterUserAsync(RegistrationModel model)
        {
            var userModel = await _userService.RegisterAsync(model);
            return Created($"api/users/{model.Username}", userModel);
        }

        private async Task<IActionResult> AuthenticateAsync(UserModel user)
        {
            var response = new AuthenticationResponse()
            {
                AccessToken = _jwtService.GenerateAccessToken(user),
                RefreshToken = _jwtService.GenerateRefreshToken(user),
            };

            await _tokenService.StoreTokenAsync(response.RefreshToken, user.Id);
            return Ok(response.Adapt<AuthenticationResponse>());
        }

        private Guid GetUserId(IEnumerable<Claim> claims)
        {
            var userIdString = _jwtService.GetClaimValue(claims, ClaimTypes.NameIdentifier);

            if (Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }

            throw new SharedException(
                ErrorTypes.Invalid,
                "Invalid token",
                $"Invalid '{ClaimTypes.NameIdentifier}' claim value"
            );
        }
    }
}