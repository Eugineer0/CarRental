namespace CarRentalDal.Models
{
    public class CarTypeCarService
    {
        public int Id { get; set; }

        public int CarTypeId { get; set; }

        public int CarServiceId { get; set; }

        public CarService CarService { get; set; } = null!;
    }
}