using SharedResources.EnumsAndConstants;

namespace CarRentalWeb.Models.Responses
{
    public class CarResponse
    {
        public string RegistrationNumber { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public string Model { get; set; } = null!;

        public int SeatPlaces { get; set; }

        public double AverageConsumption { get; set; }

        public GearboxType GearboxType { get; set; }

        public int Weight { get; set; }

        public int Length { get; set; }

        public int Power { get; set; }

        public decimal PricePerMinute { get; set; }

        public decimal PricePerHour { get; set; }

        public decimal PricePerDay { get; set; }
    }
}