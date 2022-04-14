namespace CarRentalDal.Models
{
    public class CarTypeCarServicePrices
    {
        public int Id { get; set; }

        public int CarTypeCarServiceId { get; set; }

        public Guid RentalCenterId { get; set; }
    }
}