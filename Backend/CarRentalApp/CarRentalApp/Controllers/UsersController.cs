using CarRentalApp.Models.DTOs;
using CarRentalApp.Services.Identity;
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
        [HttpPut("{username}")]
        public async Task<IActionResult> ChangeRoles(string username, RolesDTO model)
        {
            var user = await _userService.GetUserAsync(username);
            _userService.ValidateNewRoles(user, model);
            await _userService.UpdateRolesAsync(user, model);
            return Ok();
        }
    }
}