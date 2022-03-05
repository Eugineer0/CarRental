namespace CarRentalDal.Models
{
    public class Service
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<ServicePrices> Prices { get; set; } = null!;
    }
}