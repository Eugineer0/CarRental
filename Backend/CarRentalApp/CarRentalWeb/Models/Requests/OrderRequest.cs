namespace CarRentalWeb.Models.Requests
{
    public class OrderRequest
    {
        public decimal OverallPrice { get; set; }

        public DateTime StartRent { get; set; }

        public DateTime FinishRent { get; set; }

        public IEnumerable<int> OrderCarServicesId { get; set; } = null!;

        public Guid CarId { get; set; }

        public Guid RentalCenterId { get; set; }
    }
}