namespace CarRentalDal.Models
{
    public class CarTypeCarServicePrice
    {
        public int Id { get; set; }

        public Guid RentalCenterId { get; set; }

        public int CarTypeId { get; set; }

        public int CarServiceId { get; set; }

        public decimal Price { get; set; }

        public CarService CarService { get; set; } = null!;

        public CarType CarType { get; set; } = null!;
    }
}