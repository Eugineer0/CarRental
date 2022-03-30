namespace CarRentalBll.Models
{
    public class RentalCenterModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;

        public IReadOnlyCollection<CarModel> Cars { get; set; } = null!;
    }
}