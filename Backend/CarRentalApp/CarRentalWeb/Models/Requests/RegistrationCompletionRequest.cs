using System.ComponentModel.DataAnnotations;
using CarRentalWeb.Validation;

namespace CarRentalWeb.Models.Requests
{
    public class RegistrationCompletionRequest
    {
        [Required]
        [RegularExpression(
            ValidationConstants.DriverLicenseRegExp,
            ErrorMessage = ValidationConstants.DriverLicenseErrorMessage
        )]
        public string DriverLicenseSerialNumber { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;
    }
}