using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.Requests.DTOs
{
    public class RefreshTokenDTO
    {
        [Required]
        public string Token { get; set; }
    }
}
