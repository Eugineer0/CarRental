using System.ComponentModel.DataAnnotations;
using SharedResources;

namespace CarRentalWeb.Models.Requests
{
    public class RolesModificationRequest
    {
        [Required]
        public ISet<Roles> Roles { get; set; } = null!;
    }
}