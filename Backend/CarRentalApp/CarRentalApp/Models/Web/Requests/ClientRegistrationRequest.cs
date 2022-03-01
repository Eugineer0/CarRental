using System.ComponentModel.DataAnnotations;
using CarRentalApp.Validation;

namespace CarRentalApp.Models.Web.Requests
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
        [MinimumAge(minimumAge: 19, ErrorMessage = "Incorrect input: Client have to reach {1} years")]
        public DateTime? DateOfBirth { get; set; }
    }
}