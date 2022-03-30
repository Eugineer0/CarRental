using System.ComponentModel.DataAnnotations;
using SharedResources.EnumsAndConstants;

namespace CarRentalWeb.Models.Requests
{
    public class RolesModificationRequest
    {
        [Required]
        public ISet<Role> Roles { get; set; } = null!;
    }
}