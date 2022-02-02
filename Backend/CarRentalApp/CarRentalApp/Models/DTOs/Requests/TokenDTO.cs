using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.DTOs.Requests
{
    public class TokenDTO
    {
        [Required]
        public string Token { get; set; }
    }
}