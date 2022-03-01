namespace CarRentalApp.Models.Bll
{
    public class RegistrationModel
    {
        public string Email { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string PassportNumber { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string? DriverLicenseSerialNumber { get; set; }
    }
}