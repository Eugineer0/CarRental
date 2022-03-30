using System.Collections.Immutable;
using System.Security.Claims;
using CarRentalBll.Models;
using CarRentalBll.Services;
using CarRentalWeb.Models.Requests;
using CarRentalWeb.Models.Responses;
using CarRentalWeb.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using SharedResources.EnumsAndConstants;

namespace CarRentalWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly OrderService _orderService;
        private readonly JwtService _jwtService;

        public UsersController(UserService userService, OrderService orderService, JwtService jwtService)
        {
            _userService = userService;
            _orderService = orderService;
            _jwtService = jwtService;
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
            var response = await _userService
                .GetAll()
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
            return _jwtService.GetClaimValue(user.Claims, JwtRegisteredClaimNames.UniqueName);
        }

        private bool CheckIfInRoles(ClaimsPrincipal user, IEnumerable<Role> roles)
        {
            return roles.Any(role => user.IsInRole(role.ToString()));
        }
    }
}