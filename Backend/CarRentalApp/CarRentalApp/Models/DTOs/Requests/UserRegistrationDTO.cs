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
            MinimumLength = 5, 
            ErrorMessage = "Incorrect format: Username " +
            "must have length between 5 and 25 symbols"
        )]
        public string Username { get; set; }

        [Required]
        [RegularExpression(                     
            "(?=.*[0-9])" +
            "(?=.*[a-z])" +
            "(?=.*[A-Z])" +
            ".{5,25}",                        
            ErrorMessage = "Incorrect format: Password " +
            "must contain at least " +
                "one digit, " +
                "one lowercase letter, " +
                "one uppercase leter, " +
            "and has length between 5 and 25 symbols"
        )]
        public string Password { get; set; }

        [Required]
        public DateTime Birthday { get; set; }
    }
}
