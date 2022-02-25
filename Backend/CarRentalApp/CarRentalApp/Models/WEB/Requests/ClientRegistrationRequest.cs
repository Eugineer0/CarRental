using System.ComponentModel.DataAnnotations;
using CarRentalApp.ValidationAttributes;

namespace CarRentalApp.Models.WEB.Requests
{
    public class ClientRegistrationRequest : RegistrationRequestBase
    {
        [Required]
        [RegularExpression(
            "[0-9]{1}[A-Z]{2}[0-9]{6}",
            ErrorMessage = "Incorrect format: The {0} value must consist of 1 digit leading 2 capitals, followed by 6 digits"
        )]
        public string DriverLicenseSerialNumber { get; set; } = null!;

        [Required]
        [MinimumAge(minimumAge: 19, ErrorMessage = "Incorrect input: Client have to reach {1} years")]
        public DateTime? DateOfBirth { get; set; }
    }
}