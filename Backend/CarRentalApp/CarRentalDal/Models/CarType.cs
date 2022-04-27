using System.ComponentModel.DataAnnotations.Schema;
using SharedResources.EnumsAndConstants;

namespace CarRentalDal.Models
{
    public class CarType
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string Brand { get; set; } = null!;

        [Column(TypeName = "nvarchar(64)")]
        public string Model { get; set; } = null!;

        public byte SeatPlaces { get; set; }

        public double AverageConsumption { get; set; }

        public GearboxType GearboxType { get; set; }

        public int Weight { get; set; }

        public int Length { get; set; }

        public int Power { get; set; }
    }
}