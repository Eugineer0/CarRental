using CarRentalApp.Models.DTOs;
using CarRentalApp.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Controllers;

[ApiController]
[Route("api/[action]")]
public class UserController: ControllerBase
{
    private readonly RoleService _roleService;

    public UserController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpPut]
    public async Task<IActionResult> ChangeRoles([FromBody] UserDTO model)
    {
        await _roleService.UpdateRolesAsync(model);
        return Ok();
    }
}