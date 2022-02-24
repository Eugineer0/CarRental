using CarRentalApp.Contexts;
using CarRentalApp.Models.DTOs.Car;
using CarRentalApp.Models.Entities;
using Mapster;

namespace CarRentalApp.Services
{
    public class CarService
    {
        private readonly CarRentalDbContext _carRentalDbContext;

        public CarService(CarRentalDbContext carRentalDbContext)
        {
            _carRentalDbContext = carRentalDbContext;
        }

        public async Task<IEnumerable<CarDTO>> GetCars(RentalCenter rentalCenter)
        {
            return rentalCenter.Cars.Select(car => car.Adapt<Car, CarDTO>());
        }

        public async Task<IEnumerable<CarDTO>> GetAccessibleCars(RentalCenter rentalCenter, DateTime startRent, DateTime finishRent)
        {
            return rentalCenter.Cars
                .Where(car => CheckIfAccessible(car, startRent, finishRent))
                .Select(car => car.Adapt<Car, CarDTO>());
        }

        private bool CheckIfAccessible(Car car, DateTime start, DateTime finish)
        {
            return car.Orders
                .Select(order => new {order.FinishRent, order.StartRent})
                .Any(date =>
                    CheckIfBefore(date.StartRent, start)
                    || CheckIfAfter(date.FinishRent, finish)
                );
        }

        private bool CheckIfBefore(DateTime target, DateTime candidate)
        {
            return candidate.Ticks < target.Ticks;
        }

        private bool CheckIfAfter(DateTime target, DateTime candidate)
        {
            return target.Ticks < candidate.Ticks;
        }
    }
}