namespace CarRentalDal.Models
{
    public class Order
    {
        public int Id { get; set; }

        public Guid ClientId { get; set; }

        public Guid CarId { get; set; }

        public decimal OverallPrice { get; set; }

        public DateTime StartRent { get; set; }

        public DateTime FinishRent { get; set; }

        public User Client { get; set; } = null!;

        public Car Car { get; set; } = null!;

        public ICollection<OrderCarService> OrderCarServices { get; set; } = null!;
    }
}