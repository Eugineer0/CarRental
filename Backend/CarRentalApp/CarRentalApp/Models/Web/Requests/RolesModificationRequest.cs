using System.ComponentModel.DataAnnotations;
using CarRentalApp.Models.Dal;

namespace CarRentalApp.Models.Web.Requests
{
    public class RolesModificationRequest
    {
        [Required]
        public IEnumerable<Roles> Roles { get; set; } = null!;
    }
}