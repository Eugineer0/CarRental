namespace CarRentalWeb.Models.Requests
{
    public class OrderRequest
    {
        public DateTime StartRent { get; set; }

        public DateTime FinishRent { get; set; }

        public IEnumerable<int> OrderCarServicesId { get; set; } = null!;

        public string CarRegistrationNumber { get; set; } = null!;

        public string RentalCenterName { get; set; } = null!;
    }
}