using CarRentalApp.Models.DTOs;
using CarRentalApp.Services.Authentication;
using CarRentalApp.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Controllers;

[ApiController]
[Route("api/[action]")]
public class UserController: ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    //[Authorize(Roles = "SuperAdmin")]
    [HttpPut]
    public async Task<IActionResult> ChangeRoles([FromBody] UserDTO model)
    {
        await _userService.ChangeRolesAsync(model);
        return Ok();
    }
}