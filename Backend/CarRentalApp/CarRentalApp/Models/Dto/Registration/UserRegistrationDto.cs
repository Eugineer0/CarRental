using System.ComponentModel.DataAnnotations;
using CarRentalApp.ValidationAttributes;

namespace CarRentalApp.Models.Dto.Registration
{
    public class UserRegistrationDto
    {
        [Required]
        [StringLength(
            maximumLength: 254,
            MinimumLength = 3,
            ErrorMessage = "Incorrect format: The {0} value must have length between {2} and {1} characters"
        )]
        [Email(ErrorMessage = "Incorrect format: The {0} is invalid")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(
            maximumLength: 25,
            MinimumLength = 1,
            ErrorMessage = "Incorrect format: The {0} value must have length between {2} and {1} characters"
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
            ErrorMessage = "Incorrect format: The {0} value must have length between {2} and {1} characters"
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
            ErrorMessage = "Incorrect format: The {0} value must have length between {2} and {1} characters"
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
            ErrorMessage = "Incorrect format: The {0} value must have length between {2} and {1} characters"
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

        [Required]
        [MinimumAge(minimumAge: 14, ErrorMessage = "Incorrect input: Admin have to reach {1} years")]
        public virtual DateTime? DateOfBirth { get; set; }

        [RegularExpression(
            "[0-9]{1}[A-Z]{2}[0-9]{6}",
            ErrorMessage = "Incorrect format: The {0} value must consist of 1 digit leading 2 capitals, followed by 6 digits"
        )]
        public virtual string DriverLicenseSerialNumber { get; set; } = null!;
    }
}