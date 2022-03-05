using System.ComponentModel.DataAnnotations;
using CarRentalWeb.Validation;

namespace CarRentalWeb.Models.Requests
{
    public class AdminRegistrationRequest : RegistrationRequestBase
    {
        [Required]
        [MinimumAge(minimumAge: 14, ErrorMessage = "Incorrect input: Admin have to reach {1} years")]
        public DateTime? DateOfBirth { get; set; }
    }
}