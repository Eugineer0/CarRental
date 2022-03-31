namespace CarRentalDal.Models
{
    public class Order
    {
        public int Id { get; set; }

        public decimal OverallPrice { get; set; }

        public DateTime StartRent { get; set; }

        public DateTime FinishRent { get; set; }

        public ICollection<OrderCarService> OrderCarServices { get; set; } = null!;

        public Guid ClientId { get; set; }
        public User Client { get; set; } = null!;

        public Guid CarId { get; set; }

        public Car Car { get; set; } = null!;
    }
}