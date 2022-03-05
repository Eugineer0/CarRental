namespace CarRentalDal.Models
{
    public class Car
    {
        public Guid Id { get; set; }

        public string RegistrationNumber { get; set; } = null!;

        public CarType Type { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = null!;
    }
}