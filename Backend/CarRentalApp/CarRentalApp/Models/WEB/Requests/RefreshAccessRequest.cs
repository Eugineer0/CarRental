using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.WEB.Requests
{
    public class RefreshAccessRequest
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}