using System.ComponentModel.DataAnnotations;
using CarRentalApp.Models.DAL;

namespace CarRentalApp.Models.WEB.Requests
{
    public class RolesModificationRequest
    {
        [Required]
        public IEnumerable<Roles> Roles { get; set; } = null!;
    }
}