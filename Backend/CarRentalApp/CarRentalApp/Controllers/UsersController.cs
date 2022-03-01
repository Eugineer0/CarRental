using System.Collections.Immutable;
using CarRentalApp.Models.BLL;
using CarRentalApp.Models.DAL;
using CarRentalApp.Models.WEB.Requests;
using CarRentalApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Controllers
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