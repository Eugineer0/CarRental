using System.ComponentModel.DataAnnotations;
using CarRentalDal.Models;

namespace CarRentalWeb.Models.Requests
{
    public class RolesModificationRequest
    {
        [Required]
        public ISet<Roles> Roles { get; set; } = null!;
    }
}