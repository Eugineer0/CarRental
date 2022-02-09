using System.ComponentModel.DataAnnotations;
using CarRentalApp.ValidationAttributes;

namespace CarRentalApp.Models.DTOs
{
    public class UserRegistrationDTO
    {
        [Required]
        [StringLength(
            maximumLength: 254,
            MinimumLength = 3,
            ErrorMessage = "Incorrect format: The {0} value must have length between {2} and {1} characters"
        )]
        [Email(ErrorMessage = "Incorrect format: The {0} is invalid")]
        public string Email { get; set; }

        [Required]
        [StringLength(
            25,
            ErrorMessage = "Incorrect format: The {0} value cannot exceed {1} characters"
        )]
        public string Username { get; set; }

        [Required]
        [StringLength(
            maximumLength: 25,
            MinimumLength = 5,
            ErrorMessage = "Incorrect format: The {0} value must have length between {2} and {1} characters"
        )]
        [RegularExpression(
            ".*(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).*",
            ErrorMessage = "Incorrect format: The {0} value must contain at least"
                + " 1 digit, 1 lowercase letter and 1 uppercase letter"
        )]
        public string Password { get; set; }

        [Required]
        [RegularExpression(
            "[A-Z]{1}[a-z]{0,40}",
            ErrorMessage = "Incorrect format: The {0} value must start from capital and contain only latin letters"
        )]
        public string Name { get; set; }

        [Required]
        [RegularExpression(
            "[A-Z]{1}[a-z]{0,40}",
            ErrorMessage = "Incorrect format: The {0} value must start from capital and contain only latin letters"
        )]
        public string Surname { get; set; }

        [Required]
        [RegularExpression(
            "[A-Z]{2}[0-9]{7}",
            ErrorMessage = "Incorrect format: The {0} value must consist of 2 capitals leading 7 digits"
        )]
        public string PassportNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [RegularExpression(
            "[0-9]{1}[A-Z]{2}[0-9]{6}",
            ErrorMessage = "Incorrect format: The {0} value must consist of"
                + " 1 digit leading 2 capitals followed by 6 digits"
        )]
        public string? DriverLicenseSerialNumber { get; set; }
    }
}