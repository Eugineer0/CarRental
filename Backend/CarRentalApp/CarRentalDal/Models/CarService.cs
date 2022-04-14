using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalDal.Models
{
    public class CarService
    {
        public int Id { get; set; }

        [Column(TypeName="nvarchar(64)")]
        public string Name { get; set; } = null!;

        public ICollection<CarTypeCarService> CarServiceCarTypes { get; set; } = null!;
    }
}