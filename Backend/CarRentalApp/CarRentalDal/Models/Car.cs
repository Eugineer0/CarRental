using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalDal.Models
{
    public class Car
    {
        public Guid Id { get; set; }

        [Column(TypeName="nchar(7)")]
        public string RegistrationNumber { get; set; } = null!;

        public int CarTypeId { get; set; }

        public CarType CarType { get; set; } = null!;

        public Guid RentalCenterId { get; set; }

        public RentalCenter RentalCenter { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = null!;
    }
}