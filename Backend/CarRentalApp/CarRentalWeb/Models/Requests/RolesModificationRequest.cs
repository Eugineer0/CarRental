using System.ComponentModel.DataAnnotations;
using SharedResources;
using SharedResources.Enums_Constants;

namespace CarRentalWeb.Models.Requests
{
    public class RolesModificationRequest
    {
        [Required]
        public ISet<Roles> Roles { get; set; } = null!;
    }
}