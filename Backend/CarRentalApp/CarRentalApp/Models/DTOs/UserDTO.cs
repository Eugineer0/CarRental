using System.ComponentModel.DataAnnotations;
using CarRentalApp.Models.Entities;

namespace CarRentalApp.Models.DTOs;

public class UserDTO: IContainUniqueUsername
{
    public string? Email { get; set; }

    [Required]  
    public string Username { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public DateTime DateOfBirth { get; set; }
    
    public string? PassportNumber { get; set; }
    
    [Required]
    public ICollection<Roles> Roles { get; set; }
    
    public string? DriverLicenseSerialNumber { get; set; }
}