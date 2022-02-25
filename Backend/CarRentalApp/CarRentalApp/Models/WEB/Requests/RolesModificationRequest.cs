using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.WEB.Requests
{
    public class RolesModificationRequest
    {
        [Required]
        public IEnumerable<string> Roles { get; set; } = null!;
    }
}