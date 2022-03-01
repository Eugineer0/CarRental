using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.Web.Requests
{
    public class RefreshAccessRequest
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}