using CarRentalDal.Models;
using SharedResources.EnumsAndConstants;

namespace CarRentalBll.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string PassportNumber { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string? DriverLicenseSerialNumber { get; set; }

        public IEnumerable<Role> Roles { get; set; } = null!;
    }
}