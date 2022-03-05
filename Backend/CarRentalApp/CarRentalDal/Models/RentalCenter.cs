namespace CarRentalDal.Models
{
    public class RentalCenter
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;

        public IEnumerable<Car> Cars { get; set; } = null!;
    }
}