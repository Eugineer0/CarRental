using CarRentalApp.Models.DTOs;
using CarRentalApp.Services.Identity;

namespace CarRentalApp.Services.Authentication;

public class RoleService
{
    private readonly UserService _userService;

    public RoleService(UserService userService)
    {
        _userService = userService;
    }

    public async Task UpgradeRoleAsync(UserDTO userDTO)
    {
        var user = await _userService.GetExistingUserAsync(userDTO);

        await _userService.UpgradeRoleAsync(user);
    }

    public async Task DowngradeRoleAsync(UserDTO userDTO)
    {
        var user = await _userService.GetExistingUserAsync(userDTO);

        await _userService.DowngradeRoleAsync(user);
    }
}