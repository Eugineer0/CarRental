namespace CarRentalApp.Models.DTOs;

public class MinimalUserDTO
{
    public string Email { get; set; }
    
    public string Username { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public DateTime DateOfBirth { get; set; }
    
    public string PassportNumber { get; set; }
}