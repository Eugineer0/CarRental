using System;
using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.DTOs.Requests
{
    public class UserRegistrationDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(
            25,
            ErrorMessage = "Incorrect format: " +
            "The {0} value cannot exceed {1} characters"
        )]
        public string Username { get; set; }

        [Required]
        [RegularExpression(            
            "(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{5,25}",
            ErrorMessage = "Incorrect format: The {0} value must have " +
            "length between 5 and 25 characters and contain at least " +
            "one digit, one lowercase letter, one uppercase letter"
        )]
        public string Password { get; set; }

        [Required]
        public DateTime Birthday { get; set; }
    }
}
