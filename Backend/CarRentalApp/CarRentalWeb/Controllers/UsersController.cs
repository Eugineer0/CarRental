using System.Collections.Immutable;
using CarRentalBll.Models;
using CarRentalBll.Services;
using CarRentalWeb.Models.Requests;
using CarRentalWeb.Models.Responses;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedResources;

namespace CarRentalWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{userId:guid}/roles")]
        public async Task<IActionResult> ModifyRoles(Guid userId, RolesModificationRequest request)
        {
            var roles = request.Roles.ToImmutableHashSet();
            await _userService.ModifyRolesAsync(userId, roles);
            return NoContent();
        }

        [Authorize(Roles = RolesConstants.AdminRolesString)]
        [HttpPost("{userId:guid}/approve-client")]
        public async Task<IActionResult> ApproveClient(Guid userId)
        {
            await _userService.ApproveClientAsync(userId);
            return Ok();
        }

        [Authorize(Roles = RolesConstants.AdminRolesString)]
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var userModel = await _userService.GetByAsync(userId);
            var response = Convert(userModel);
            return Ok(response);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{userId:guid}/orders")]
        public async Task<IActionResult> GetUserOrders(Guid userId)
        {
            var orderModels = _userService.GetOrdersBy(userId);
            var response = await orderModels
                .Select(orderModel => orderModel.Adapt<OrderResponse>())
                .ToListAsync();
            return Ok(response);
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

        private UserResponse Convert(UserModel userModel)
        {
            var userResponse = userModel.Adapt<UserResponse>();
            userResponse.ApprovalRequested = _userService.CheckIfApprovalRequested(userModel);

            return userResponse;
        }
    }
}