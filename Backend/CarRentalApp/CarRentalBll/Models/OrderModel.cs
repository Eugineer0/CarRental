namespace CarRentalBll.Models;

public class OrderModel
{
    public decimal OverallPrice { get; set; }

    public DateTime StartRent { get; set; }

    public DateTime FinishRent { get; set; }

    public ICollection<OrderServiceModel> OrderServices { get; set; } = null!;

    public Guid UserId { get; set; }

    public CarModel Car { get; set; } = null!;

    public RentalCenterModel RentalCenter { get; set; } = null!;
}