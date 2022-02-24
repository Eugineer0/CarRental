namespace CarRentalApp.Models.Entities;

public class Service
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<ServicePrices> Prices { get; set; }
}