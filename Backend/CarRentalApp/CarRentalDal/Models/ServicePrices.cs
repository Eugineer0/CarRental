namespace CarRentalDal.Models
{
    public class ServicePrices
    {
        public int Id { get; set; }

        public Service Service { get; set; } = null!;

        public int CarTypeId { get; set; }
    }
}