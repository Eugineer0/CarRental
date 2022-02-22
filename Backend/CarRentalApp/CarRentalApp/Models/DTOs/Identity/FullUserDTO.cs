using CarRentalApp.Models.Entities;

namespace CarRentalApp.Models.DTOs;

public class FullUserDTO: MinimalUserDTO
{
    public string DriverLicenseSerialNumber { get; set; }
    
    public ICollection<Roles> Roles { get; set; }
}