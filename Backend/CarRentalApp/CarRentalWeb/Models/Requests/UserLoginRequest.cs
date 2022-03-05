using System.ComponentModel.DataAnnotations;

namespace CarRentalWeb.Models.Requests
{
    public class UserLoginRequest
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}