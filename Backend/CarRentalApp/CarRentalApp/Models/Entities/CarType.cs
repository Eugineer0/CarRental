namespace CarRentalApp.Models.Entities;

public class CarType
{
    public int Id { get; set; }

    public string Brand { get; set; }

    public string Model { get; set; }

    public int SeatPlaces { get; set; }

    public double AverageConsumption { get; set; }

    public GearboxTypes GearboxType { get; set; }

    public int Weight { get; set; }

    public int Length { get; set; }

    public int Power { get; set; }

    public decimal PricePerMinute { get; set; }

    public decimal PricePerHour { get; set; }

    public decimal PricePerDay { get; set; }

    public ICollection<ServicePrices> ServicePrices { get; set; }
}

public enum GearboxTypes : byte
{
    Mechanical,
    Automatic,
    Robotic
}