using System;
using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.DTOs.Requests
{
    public class UserRegistrationDTO : UserLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

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
        public override string Password { 
            get => base.Password;
            set => base.Password = value; 
        }

        [Required]
        public DateTime Birthday { get; set; }
    }
}
