namespace CarRentalApp.Models.Entities;

public class ServicePrices
{
    public int Id { get; set; }

    public int ServiceId { get; set; }
    
    public int CarTypeId { get; set; }
}