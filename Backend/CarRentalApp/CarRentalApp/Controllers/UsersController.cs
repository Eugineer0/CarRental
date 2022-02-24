using CarRentalApp.Models.Dto;
using CarRentalApp.Models.Entities;
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
        public async Task<IActionResult> ModifyRoles(string username, RolesDto model)
        {
            await _userService.ModifyUserRolesAsync(username, model);
            return Ok();
        }

        [Authorize(Roles = UserRole.AdminRolesString)]
        [HttpPut("{username}/[action]")]
        public async Task<IActionResult> ApproveClient(string username)
        {
            await _userService.ApproveClientAsync(username);
            return Ok();
        }
    }
}