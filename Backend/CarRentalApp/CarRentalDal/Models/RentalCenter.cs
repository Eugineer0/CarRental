using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalDal.Models
{
    public class RentalCenter
    {
        public Guid Id { get; set; }

        [Column(TypeName="nvarchar(32)")]
        public string Name { get; set; } = null!;

        [Column(TypeName="nvarchar(64)")]
        public string Country { get; set; } = null!;

        [Column(TypeName="nvarchar(64)")]
        public string City { get; set; } = null!;

        [Column(TypeName="nvarchar(128)")]
        public string Address { get; set; } = null!;

        public IEnumerable<Car> Cars { get; set; } = null!;
    }
}