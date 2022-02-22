namespace CarRentalApp.Models.Entities;

public class Order
{
    public int Id { get; set; }

    public DateTime StartRent { get; set; }
    
    public DateTime FinishRent { get; set; }
    
    public ICollection<OrderService> OrderServices { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid CarId { get; set; }
    
    public Guid RentalCenterId { get; set; }
}