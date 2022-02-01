using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.DTOs.Requests
{
    public class RefreshTokenDTO
    {
        [Required]
        public string Token { get; set; }
    }
}
