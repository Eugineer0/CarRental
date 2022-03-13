using System.ComponentModel.DataAnnotations;
using CarRentalWeb.Validation;

namespace CarRentalWeb.Models.Requests
{
    public class AdminRegistrationRequest : RegistrationRequestBase
    {
        [Required]
        public DateTime? DateOfBirth { get; set; }
    }
}