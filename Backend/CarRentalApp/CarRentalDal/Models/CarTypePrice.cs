namespace CarRentalDal.Models
{
    public class CarTypePrice
    {
        public int Id { get; set; }

        public int CarTypeId { get; set; }

        public Guid RentalCenterId { get; set; }

        public decimal PricePerMinute { get; set; }

        public decimal PricePerHour { get; set; }

        public decimal PricePerDay { get; set; }
    }
}