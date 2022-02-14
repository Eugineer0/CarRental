using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public byte[] HashedPassword { get; set; }

        public byte[] Salt { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PassportNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public ICollection<UserRole> Roles { get; set; }

        public string? DriverLicenseSerialNumber { get; set; }
    }
}