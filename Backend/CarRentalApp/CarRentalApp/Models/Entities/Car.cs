namespace CarRentalApp.Models.Entities;

public class Car
{
    public Guid Id { get; set; }

    public string RegistrationNumber { get; set; }

    public CarType Type { get; set; }
    
    public ICollection<Order> Orders { get; set; }
}