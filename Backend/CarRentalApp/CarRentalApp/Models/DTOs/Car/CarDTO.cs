using CarRentalApp.Models.Entities;

namespace CarRentalApp.Models.DTOs.Car;

public class CarDTO
{
    public string RegistrationNumber { get; set; }

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

    public IEnumerable<ServiceResponse> AvailableServices { get; set; }
}