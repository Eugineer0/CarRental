namespace CarRentalWeb.Models.Responses
{
    public class RentalCenterResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int AvailableCarsNumber { get; set; }
    }
}