using System.ComponentModel.DataAnnotations;

namespace CarRentalWeb.Models.Requests
{
    public class RefreshAccessRequest
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}