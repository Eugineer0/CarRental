using CarRentalBll.Models.Cars;

namespace CarRentalBll.Models.RentalCenters
{
    public class OrderModel
    {
        public decimal OverallPrice { get; set; }

        public DateTime StartRent { get; set; }

        public DateTime FinishRent { get; set; }

        public ICollection<CarServiceModel> CarServices { get; set; } = null!;

        public UserModel Client { get; set; } = null!;

        public CarModel Car { get; set; } = null!;

        public RentalCenterModel RentalCenter { get; set; } = null!;
    }
}