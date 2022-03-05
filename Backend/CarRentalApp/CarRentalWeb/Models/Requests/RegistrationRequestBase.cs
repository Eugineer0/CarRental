using System.ComponentModel.DataAnnotations;
using CarRentalWeb.Validation;

namespace CarRentalWeb.Models.Requests
{
    public abstract class RegistrationRequestBase
    {
        [Required]
        [StringLength(
            maximumLength: 254,
            MinimumLength = 3,
            ErrorMessage = ValidationConstants.StringLengthErrorMessage
        )]
        [Email(ErrorMessage = "Incorrect format: The {0} is invalid")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(
            maximumLength: 25,
            MinimumLength = 1,
            ErrorMessage = ValidationConstants.StringLengthErrorMessage
        )]
        [RegularExpression(
            "\\w*",
            ErrorMessage = "Incorrect format: The {0} value must contain only latin letters, digits and underscore"
        )]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(
            maximumLength: 25,
            MinimumLength = 5,
            ErrorMessage = ValidationConstants.StringLengthErrorMessage
        )]
        [RegularExpression(
            ".*(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).*",
            ErrorMessage = "Incorrect format: The {0} value must contain at least 1 digit, 1 lowercase letter and 1 uppercase letter"
        )]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(
            maximumLength: 64,
            MinimumLength = 1,
            ErrorMessage = ValidationConstants.StringLengthErrorMessage
        )]
        [RegularExpression(
            "[A-Z]{1}[a-z]*((-[A-Z])?[a-z]*)*",
            ErrorMessage = "Incorrect format: The {0} value must start from capital and contain only latin letters or hyphen, followed by capital"
        )]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(
            maximumLength: 64,
            MinimumLength = 1,
            ErrorMessage = ValidationConstants.StringLengthErrorMessage
        )]
        [RegularExpression(
            "[A-Z]{1}[a-z]*((-[A-Z])?[a-z]*)*",
            ErrorMessage = "Incorrect format: The {0} value must start from capital and contain only latin letters or hyphen, followed by capital"
        )]
        public string Surname { get; set; } = null!;

        [Required]
        [RegularExpression(
            "[A-Z]{2}[0-9]{7}",
            ErrorMessage = "Incorrect format: The {0} value must consist of 2 capitals leading 7 digits"
        )]
        public string PassportNumber { get; set; } = null!;
    }
}