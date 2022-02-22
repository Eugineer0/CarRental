using AutoMapper;
using CarRentalApp.Contexts;
using CarRentalApp.Models.DTOs.RentalCenter;
using CarRentalApp.Models.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Services
{
    public class RentalCenterService
    {
        private readonly IMapper _rentalCenterMapper;
        private readonly CarRentalDbContext _carRentalDbContext;
        private readonly CarService _carService;

        public RentalCenterService(CarRentalDbContext carRentalDbContext, IMapper rentalCenterMapper, CarService carService)
        {
            _carRentalDbContext = carRentalDbContext;
            _rentalCenterMapper = rentalCenterMapper;
            _carService = carService;
        }

        public Task<RentalCenter> GetCenterAsync(string name)
        {
            return _carRentalDbContext.RentalCenters
                .Where(center => center.Name.Equals(name))
                .Include(center => center.Cars)
                .ThenInclude(car => car.Orders)
                .FirstAsync();
        }

        public async Task<IEnumerable<RentalCenterDTO>> GetAllCenters()
        {
            return _carRentalDbContext.RentalCenters
                .Include(center => center.Cars)
                .Select(center => _rentalCenterMapper.Map<RentalCenter, RentalCenterDTO>(center));
        }

        public async Task<IEnumerable<RentalCenterDTO>> GetCentersFilteredByDate(DateTime start, DateTime finish)
        {
            var centerTasks = _carRentalDbContext.RentalCenters
                .Include(center => center.Cars)
                .ThenInclude(car => car.Orders)
                .Select(center => ConvertToDTO(center, start, finish));
            
            IEnumerable<RentalCenterDTO> centers = await Task.WhenAll(centerTasks);

            return centers;
        }
        
        private async Task<RentalCenterDTO> ConvertToDTO(RentalCenter rentalCenter, DateTime start, DateTime finish)
        {
            var rentalCenterDTO = rentalCenter.Adapt<RentalCenter, RentalCenterDTO>();
            rentalCenterDTO.AccessibleCarsNumber = (await _carService.GetAccessibleCars(rentalCenter, start, finish)).Count();

            return rentalCenterDTO;
        }


    }
}