using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.Dto.Registration;

public class CompleteRegistrationDto
{
    [Required]
    [RegularExpression(
        "[0-9]{1}[A-Z]{2}[0-9]{6}",
        ErrorMessage = "Incorrect format: The {0} value must consist of 1 digit leading 2 capitals followed by 6 digits"
    )]
    public string DriverLicenseSerialNumber { get; set; } = null!;

    [Required]
    public string Token { get; set; } = null!;
}