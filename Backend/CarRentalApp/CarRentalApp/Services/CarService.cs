using AutoMapper;
using CarRentalApp.Contexts;
using CarRentalApp.Models.DTOs.Car;
using CarRentalApp.Models.Entities;

namespace CarRentalApp.Services
{
    public class CarService
    {
        private readonly IMapper _carMapper;
        private readonly CarRentalDbContext _carRentalDbContext;

        public CarService(CarRentalDbContext carRentalDbContext, IMapper carMapper)
        {
            _carRentalDbContext = carRentalDbContext;
            _carMapper = carMapper;
        }

        public async Task<IEnumerable<CarDTO>> GetCars(RentalCenter rentalCenter)
        {
            return rentalCenter.Cars.Select(car => _carMapper.Map<Car, CarDTO>(car));
        }

        public async Task<IEnumerable<CarDTO>> GetAccessibleCars(RentalCenter rentalCenter, DateTime startRent, DateTime finishRent)
        {
            return rentalCenter.Cars
                .Where(car => CheckIfAccessible(car, startRent, finishRent))
                .Select(car => _carMapper.Map<Car, CarDTO>(car));
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