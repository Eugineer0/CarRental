namespace CarRentalBll.Models.Cars
{
    public class CarModel
    {
        public Guid Id { get; set; }

        public string RegistrationNumber { get; set; } = null!;

        public Guid RentalCenterId { get; set; }

        public CarTypeModel CarType { get; set; } = null!;
    }
}