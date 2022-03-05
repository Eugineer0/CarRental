namespace CarRentalWeb.Models.Responses;

public class OrderResponse
{
    public decimal OverallPrice { get; set; }

    public DateTime StartRent { get; set; }

    public DateTime FinishRent { get; set; }

    public ICollection<OrderServiceResponse> OrderServices { get; set; } = null!;

    public Guid UserId { get; set; }

    public CarResponse Car { get; set; } = null!;

    public RentalCenterResponse RentalCenter { get; set; } = null!;
}