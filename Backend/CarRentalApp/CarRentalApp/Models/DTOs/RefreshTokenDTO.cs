using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.DTOs
{
    public class RefreshTokenDTO
    {
        [Required]
        public string Token { get; set; }
    }
}