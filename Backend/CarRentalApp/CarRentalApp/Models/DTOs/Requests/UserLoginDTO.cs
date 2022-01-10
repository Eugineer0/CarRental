using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.DTOs.Requests
{
    public class UserLoginDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression(
            @"^" +
            "(?=.*[A-Z])" + // At least one UpperCase
            "(?=.*[0-9])" + // At least one Digit
            "(?=.*[a-z])" + // At least one LowerCase
            ".{5,25}" +     // Length between 5 and 25
            "$",
         ErrorMessage = "Incorrect format")]
        public string Password { get; set; }
    }
}