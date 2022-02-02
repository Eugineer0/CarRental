using System.ComponentModel.DataAnnotations;
using CarRentalApp.ValidationAttributes;

namespace CarRentalApp.Models.DTOs.Requests
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
                + " one digit, one lowercase letter and one uppercase letter"
        )]
        public string Password { get; set; }

        [Required]
        public DateTime Birthday { get; set; }
    }
}