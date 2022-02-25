using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.WEB.Requests
{
    public class UserLoginRequest
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}