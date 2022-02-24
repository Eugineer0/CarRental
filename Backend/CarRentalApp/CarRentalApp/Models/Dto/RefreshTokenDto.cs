using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.Dto
{
    public class RefreshTokenDto
    {
        [Required]
        public string Token { get; set; } = null!;
    }
}