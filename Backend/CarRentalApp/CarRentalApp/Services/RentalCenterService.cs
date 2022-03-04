using CarRentalApp.Contexts;
using CarRentalApp.Exceptions;
using CarRentalApp.Models.BLL;
using CarRentalApp.Models.DTOs.RentalCenter;
using CarRentalApp.Models.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalApp.Services
{
    public class RentalCenterService
    {
        private readonly CarRentalDbContext _carRentalDbContext;
        private readonly CarService _carService;

        public RentalCenterService(CarRentalDbContext carRentalDbContext, CarService carService)
        {
            _carRentalDbContext = carRentalDbContext;
            _carService = carService;
        }

        public async Task<IEnumerable<RentalCenterModel>> GetCenterModelsAsync()
        {
            var centers = await AddCarsInfoAsync(_carRentalDbContext.RentalCenters);
            return centers.Select(ConvertToModel);
        }

        public async Task<RentalCenterModel> GetCenterModelAsync(string name)
        {
            var centers = await GetCentersAsync(name);
            if (!centers.Any())
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "Rental center not found",
                    "Rental center with such username not found"
                );
            }
            centers = await AddCarsInfoAsync(centers);
            var center = await centers.FirstAsync();

            return ConvertToModel(center);
        }

        public async Task<IEnumerable<RentalCenterModel>> GetFilteredCenterModelsAsync(CenterFilterRequest request)
        {
            var queryCenters = await AddCarsInfoAsync(_carRentalDbContext.RentalCenters);
            IEnumerable<RentalCenter> centers = await AddOrdersInfoAsync(queryCenters);

            if (!request.Country.IsNullOrEmpty())
            {
                centers = FilterByCountry(centers, request.Country);
            }

            if (!request.City.IsNullOrEmpty())
            {
                centers = FilterByCity(centers, request.City);
            }

            if (request.StartRent.HasValue && request.FinishRent.HasValue)
            {
                centers = FilterByDate(centers, request.StartRent.Value, request.FinishRent.Value);
            }

            if (request.MinimumAvailableCarsNumber > 0)
            {
                centers = FilterByAccessibleCarsNumberAsync(centers, request.MinimumAvailableCarsNumber);
            }

            return centers.Select(ConvertToModel);
        }

        public async Task<IEnumerable<CarModel>> GetCenterCarModelsAsync(string name)
        {
            var centers = await GetCentersAsync(name);
            centers = await AddCarsInfoAsync(centers);
            var center = await centers.FirstAsync();

            return _carService.GetCars(center.Cars);
        }

        private async Task<IQueryable<RentalCenter>> GetCentersAsync(string name)
        {
            return _carRentalDbContext.RentalCenters
                .Where(center => center.Name.Equals(name));
        }

        private async Task<IQueryable<RentalCenter>> AddOrdersInfoAsync(IQueryable<RentalCenter> centers)
        {
            return centers.Include(center => center.Cars)
                .ThenInclude(car => car.Orders);
        }

        private async Task<IQueryable<RentalCenter>> AddCarsInfoAsync(IQueryable<RentalCenter> centers)
        {
            return centers
                .Include(center => center.Cars)
                .ThenInclude(car => car.Type)
                .ThenInclude(type => type.ServicePrices)
                .ThenInclude(servicePrice => servicePrice.Service);
        }

        private IEnumerable<RentalCenter> FilterByDate(IEnumerable<RentalCenter> rentalCenters, DateTime start, DateTime finish)
        {
            return rentalCenters
                .ForEach(
                    center => center.Cars = center.Cars.Where(car => _carService.CheckIfAvailable(car, start, finish))
                );
        }

        private IEnumerable<RentalCenter> FilterByCountry(IEnumerable<RentalCenter> rentalCenters, string country)
        {
            return rentalCenters.Where(center => center.Country.Equals(country));
        }

        private IEnumerable<RentalCenter> FilterByCity(IEnumerable<RentalCenter> rentalCenters, string city)
        {
            return rentalCenters.Where(center => center.City.Equals(city));
        }

        private IEnumerable<RentalCenter> FilterByAccessibleCarsNumberAsync(IEnumerable<RentalCenter> rentalCenters, int minimumNumber)
        {
            return rentalCenters.Where(center => center.Cars.Count() >= minimumNumber);
        }

        private RentalCenterModel ConvertToModel(RentalCenter rentalCenter)
        {
            var model = rentalCenter.Adapt<RentalCenterModel>();
            model.Cars = rentalCenter.Cars.Select(car => car.Adapt<CarModel>()).ToList();

            return model;
        }
    }

    public static class LINQExtension
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
                yield return item;
            }
        }
    }
}