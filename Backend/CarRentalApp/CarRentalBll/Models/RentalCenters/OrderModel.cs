using CarRentalBll.Models.Cars;
using CarRentalBll.Models.RentalCenters;

namespace CarRentalBll.Models
{
    public class OrderModel
    {
        public DateTime StartRent { get; set; }

        public DateTime FinishRent { get; set; }

        public ICollection<CarServiceModel> CarServices { get; set; } = null!;

        public UserModel Client { get; set; } = null!;

        public CarModel Car { get; set; } = null!;

        public RentalCenterModel RentalCenter { get; set; } = null!;
    }
}