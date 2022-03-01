using System.ComponentModel.DataAnnotations;
using CarRentalApp.Validation;

namespace CarRentalApp.Models.Web.Requests
{
    public class AdminRegistrationRequest : RegistrationRequestBase
    {
        [Required]
        [MinimumAge(minimumAge: 14, ErrorMessage = "Incorrect input: Admin have to reach {1} years")]
        public DateTime? DateOfBirth { get; set; }
    }
}