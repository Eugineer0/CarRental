using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.DTOs.Registration;

public class CompleteRegistrationDTO
{
    [Required]
    [RegularExpression(
        "[0-9]{1}[A-Z]{2}[0-9]{6}",
        ErrorMessage = "Incorrect format: The {0} value must consist of"
                       + " 1 digit leading 2 capitals followed by 6 digits"
    )]
    public string DriverLicenseSerialNumber { get; set; }
    
    [Required]
    public string Token { get; set; }
}