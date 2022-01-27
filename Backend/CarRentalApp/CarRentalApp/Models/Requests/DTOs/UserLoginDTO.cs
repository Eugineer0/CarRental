using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.Requests.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]        
        public string Password { get; set; }
    }
}