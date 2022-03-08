namespace CarRentalDal.Models
{
    public class CarServicePrice
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public CarService CarService { get; set; } = null!;

        public int CarTypeId { get; set; }
    }
}