namespace CarRentalWeb.Models.Responses
{
    public class CarServiceResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
    }
}