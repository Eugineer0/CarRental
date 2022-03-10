using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarRentalBll.Models;
using CarRentalBll.Models.RentalCenters;
using CarRentalBll.Services;
using CarRentalWeb.Models.Requests;
using CarRentalWeb.Models.Responses;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedResources;
using SharedResources.Enums_Constants;

namespace CarRentalWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TokenService _tokenService;
        private readonly OrderService _orderService;

        public UsersController(UserService userService, TokenService tokenService, OrderService orderService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _orderService = orderService;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{username}/roles")]
        public async Task<IActionResult> ModifyRoles(string username, RolesModificationRequest request)
        {
            var roles = request.Roles.ToImmutableHashSet();
            await _userService.ModifyRolesAsync(username, roles);
            return NoContent();
        }

        [Authorize(Roles = RolesConstants.AdminRolesString)]
        [HttpPost("{username}/approve-client")]
        public async Task<IActionResult> ApproveClient(string username)
        {
            await _userService.ApproveClientAsync(username);
            return Ok();
        }

        [Authorize(Roles = RolesConstants.AdminRolesString)]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var userModel = await _userService.GetByAsync(username);
            var response = Convert(userModel);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("{username}/orders")]
        public async Task<IActionResult> GetUserOrders(string username)
        {
            if (CheckIfInRoles(this.User, RolesConstants.AdminRoles) || username == GetCurrentUsername(this.User))
            {
                var orderModels = _orderService.GetOrdersBy(username);
                var response = await orderModels
                    .Select(orderModel => orderModel.Adapt<OrderResponse>())
                    .ToListAsync();
                return Ok(response);
            }

            return Forbid();
        }

        [Authorize(Roles = RolesConstants.ClientRolesString)]
        [HttpPut("{username}/orders")]
        public async Task<IActionResult> MakeOrder(string username, OrderRequest orderRequest)
        {
            if (username != GetCurrentUsername(this.User))
            {
                return Forbid();
            }

            var model = orderRequest.Adapt<OrderModel>();
            _orderService.ValidateOrderAsync(model);
            await _orderService.CreateOrderAsync(model);

            return Created($"{username}/orders", model);
        }

        [Authorize(Roles = RolesConstants.AdminRolesString)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var userModels = _userService.GetAll();
            var response = await userModels
                .Select(userModel => Convert(userModel))
                .ToListAsync();

            return Ok(response);
        }

        private static UserResponse Convert(UserModel userModel)
        {
            var userResponse = userModel.Adapt<UserResponse>();
            userResponse.ApprovalRequested = UserService.CheckIfApprovalRequested(userModel);

            return userResponse;
        }

        private string GetCurrentUsername(ClaimsPrincipal user)
        {
            return _tokenService.GetClaimValue(user.Claims, JwtRegisteredClaimNames.UniqueName);
        }

        private bool CheckIfInRoles(ClaimsPrincipal user, IEnumerable<Roles> roles)
        {
            return roles.Any(role => user.IsInRole(role.ToString()));
        }
    }
}