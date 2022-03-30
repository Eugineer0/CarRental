using System.ComponentModel.DataAnnotations;
using CarRentalWeb.Validation;
using SharedResources.EnumsAndConstants;

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
        public DateTime? DateOfBirth { get; set; }
    }
}