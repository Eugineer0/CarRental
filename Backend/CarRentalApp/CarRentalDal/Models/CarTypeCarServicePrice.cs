namespace CarRentalDal.Models
{
    public class CarTypeCarServicePrice
    {
        public int Id { get; set; }

        public Guid RentalCenterId { get; set; }

        public int CarTypeCarServiceId { get; set; }

        public decimal Price { get; set; }

        public CarTypeCarService CarTypeCarService { get; set; } = null!;
    }
}