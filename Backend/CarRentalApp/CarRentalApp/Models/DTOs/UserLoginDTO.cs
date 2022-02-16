using System.ComponentModel.DataAnnotations;

namespace CarRentalApp.Models.DTOs
{
    public class UserLoginDTO: IContainUniqueUsername
    {
        [Required]
        public string Username { get; set; }

        [Required]        
        public string Password { get; set; }
    }
}