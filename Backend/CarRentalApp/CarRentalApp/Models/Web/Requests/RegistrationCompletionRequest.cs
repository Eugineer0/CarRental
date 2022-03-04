using System.ComponentModel.DataAnnotations;
using CarRentalApp.Validation;

namespace CarRentalApp.Models.Web.Requests
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