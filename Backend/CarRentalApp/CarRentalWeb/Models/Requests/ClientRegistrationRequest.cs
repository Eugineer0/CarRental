using System.ComponentModel.DataAnnotations;
using CarRentalWeb.Validation;

namespace CarRentalWeb.Models.Requests
{
    public class ClientRegistrationRequest : RegistrationRequestBase
    {
        [Required]
        [RegularExpression(
            ValidationConstants.DriverLicenseRegExp,
            ErrorMessage = ValidationConstants.DriverLicenseErrorMessage
        )]
        public string DriverLicenseSerialNumber { get; set; } = null!;

        [Required]
        [MinimumAge(ErrorMessage = "Incorrect input: Client have to reach {1} years")]
        public DateTime? DateOfBirth { get; set; }
    }
}