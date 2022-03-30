using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalDal.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Column(TypeName="nvarchar(254)")]
        public string Email { get; set; } = null!;

        [Column(TypeName="nvarchar(25)")]
        public string Username { get; set; } = null!;

        [Column(TypeName="binary(32)")]
        public byte[] HashedPassword { get; set; } = null!;

        [Column(TypeName="binary(32)")]
        public byte[] Salt { get; set; } = null!;

        [Column(TypeName="nvarchar(64)")]
        public string Name { get; set; } = null!;

        [Column(TypeName="nvarchar(64)")]
        public string Surname { get; set; } = null!;

        [Column(TypeName="nchar(9)")]
        public string PassportNumber { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        [Column(TypeName="nchar(9)")]
        public string? DriverLicenseSerialNumber { get; set; }

        public List<UserRole> Roles { get; set; } = null!;
    }
}