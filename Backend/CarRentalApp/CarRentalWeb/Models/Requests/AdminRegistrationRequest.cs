using System.ComponentModel.DataAnnotations;
using CarRentalWeb.Validation;

namespace CarRentalWeb.Models.Requests
{
    public class AdminRegistrationRequest : RegistrationRequestBase
    {
        [Required]
        //[MinimumAge(ErrorMessage = "Incorrect input: Admin have to reach {1} years")]
        public DateTime? DateOfBirth { get; set; }
    }
}