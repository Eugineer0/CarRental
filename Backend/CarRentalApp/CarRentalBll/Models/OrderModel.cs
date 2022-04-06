namespace CarRentalBll.Models
{
    public class OrderModel
    {
        public decimal OverallPrice { get; set; }

        public DateTime StartRent { get; set; }

        public DateTime FinishRent { get; set; }

        public ICollection<CarServiceModel> OrderCarServices { get; set; } = null!;

        public Guid ClientId { get; set; }

        public CarModel Car { get; set; } = null!;

        public RentalCenterModel RentalCenter { get; set; } = null!;
    }
}