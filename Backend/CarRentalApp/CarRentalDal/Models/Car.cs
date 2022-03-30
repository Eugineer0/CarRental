using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalDal.Models
{
    public class Car
    {
        public Guid Id { get; set; }

        [Column(TypeName="nchar(7)")]
        public string RegistrationNumber { get; set; } = null!;

        public CarType Type { get; set; } = null!;

        public RentalCenter RentalCenter { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = null!;
    }
}