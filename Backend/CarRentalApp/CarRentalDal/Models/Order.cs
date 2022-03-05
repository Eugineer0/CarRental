namespace CarRentalDal.Models
{
    public class Order
    {
        public int Id { get; set; }

        public decimal OverallPrice { get; set; }

        public DateTime StartRent { get; set; }

        public DateTime FinishRent { get; set; }

        public ICollection<OrderService> OrderServices { get; set; } = null!;

        public Guid UserId { get; set; }

        public Car Car { get; set; } = null!;

        public RentalCenter RentalCenter { get; set; } = null!;
    }
}