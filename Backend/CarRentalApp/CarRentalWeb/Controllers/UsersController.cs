using System.Collections.Immutable;
using CarRentalBll.Models;
using CarRentalBll.Services;
using CarRentalWeb.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPut("{username}/roles")]
        public async Task<IActionResult> ModifyRoles(string username, RolesModificationRequest request)
        {
            var roles = request.Roles.ToImmutableHashSet();
            await _userService.ModifyRolesAsync(username, roles);
            return NoContent();
        }

        [Authorize(Roles = RolesInfo.AdminRolesString)]
        [HttpPost("{username}/approve-client")]
        public async Task<IActionResult> ApproveClient(string username)
        {
            await _userService.ApproveClientAsync(username);
            return Ok();
        }
    }
}