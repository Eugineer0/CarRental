namespace CarRentalApp.Models.Entities;

public class ServicePrices
{
    public int Id { get; set; }

    public Service Service { get; set; }

    public int CarTypeId { get; set; }
}