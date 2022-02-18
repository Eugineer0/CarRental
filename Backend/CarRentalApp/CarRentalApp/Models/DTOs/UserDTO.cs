using CarRentalApp.Models.Entities;

namespace CarRentalApp.Models.DTOs;

public class UserDTO
{
    public string Email { get; set; }
    
    public string Username { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public DateTime DateOfBirth { get; set; }
    
    public string PassportNumber { get; set; }

    public string DriverLicenseSerialNumber { get; set; }
    
    public ICollection<Roles> Roles { get; set; }
}