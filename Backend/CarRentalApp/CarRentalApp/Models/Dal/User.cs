namespace CarRentalApp.Models.Dal
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;

        public byte[] HashedPassword { get; set; } = null!;

        public byte[] Salt { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string PassportNumber { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string? DriverLicenseSerialNumber { get; set; }

        public List<UserRole> Roles { get; set; } = null!;
    }
}