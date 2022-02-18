using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.DTOs;

public class UserDTO: RolesDTO
{
    public string Email { get; set; }
    
    public string Username { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public DateTime DateOfBirth { get; set; }
    
    public string PassportNumber { get; set; }

    public string DriverLicenseSerialNumber { get; set; }
}