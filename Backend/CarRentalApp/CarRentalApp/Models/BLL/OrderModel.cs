namespace CarRentalApp.Models.BLL;

public class OrderModel
{
    public decimal OverallPrice { get; set; }

    public DateTime StartRent { get; set; }

    public DateTime FinishRent { get; set; }

    public ICollection<ServiceModel> Services { get; set; }

    public CarModel Car { get; set; }
}