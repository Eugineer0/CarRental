using CarRentalApp.Models.DTOs;
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
        public async Task<IActionResult> ChangeRoles(string username, RolesDTO model)
        {
            var user = await _userService.GetUserAsync(username);
            var roles = _userService.ValidateNewRoles(user, model);
            await _userService.UpdateRolesAsync(user, roles);
            return Ok();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MinimalUserDTO>>> GetAllUsers()
        {
            return Ok(await _userService.GetAllMinimalUserDTOsAsync());
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{username}")]
        public async Task<ActionResult<FullUserDTO>> GetUser(string username)
        {
            return Ok(await _userService.GetFullUserDTOAsync(username));
        }
    }
}