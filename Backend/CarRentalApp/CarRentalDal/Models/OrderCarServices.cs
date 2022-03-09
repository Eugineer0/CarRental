namespace CarRentalDal.Models
{
    public class OrderCarService
    {
        public int Id { get; set; }

        public int CarServiceId { get; set; }

        public CarService CarService { get; set; } = null!;

        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;
    }
}