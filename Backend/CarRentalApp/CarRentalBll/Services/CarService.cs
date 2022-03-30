using CarRentalBll.Models;
using CarRentalDal.Contexts;
using CarRentalDal.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SharedResources.Exceptions;
using SharedResources.Helpers;

namespace CarRentalBll.Services
{
    public class CarService
    {
        private readonly CarRentalDbContext _carRentalDbContext;

        public CarService(CarRentalDbContext carRentalDbContext)
        {
            _carRentalDbContext = carRentalDbContext;
        }

        public async Task<CarModel> GetModelByAsync(Guid carId)
        {
            var car = await _carRentalDbContext.Cars
                .Include(car => car.Orders)
                .Include(car => car.Type)
                .FirstOrDefaultAsync(car => car.Id == carId);
            if (car == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "Car not found",
                    "Car with such username not found"
                );
            }

            return car.Adapt<CarModel>();
        }

        public async Task<Car> GetByAsync(Guid carId)
        {
            var car = await _carRentalDbContext.Cars
                .Include(car => car.Orders)
                .FirstOrDefaultAsync(car => car.Id == carId);
            if (car == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "Car not found",
                    "Car with such username not found"
                );
            }

            return car;
        }

        public IEnumerable<CarModel> GetAccessible(IEnumerable<Car> cars, DateTime startRent, DateTime finishRent)
        {
            return cars.Where(car => CheckIfAvailable(car, startRent, finishRent))
                .Select(car => car.Adapt<CarModel>());
        }

        public bool CheckIfAvailable(Car car, DateTime start, DateTime finish)
        {
            return car.Orders
                .Select(order => new { order.FinishRent, order.StartRent })
                .All(period => finish < period.StartRent || period.FinishRent > start);
        }

        public async Task<decimal> GetRentalPriceAsync(Guid carId, DateTime startRent, DateTime finishRent)
        {
            var car = await GetByAsync(carId);

            var period = startRent.GetPeriod(finishRent);

            return period.Days * car.Type.PricePerDay
                + period.Hours * car.Type.PricePerHour
                + period.Minutes * car.Type.PricePerMinute;
        }
    }
}