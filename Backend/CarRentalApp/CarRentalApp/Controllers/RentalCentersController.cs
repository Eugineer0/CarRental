using CarRentalApp.Models.DTOs.Car;
using CarRentalApp.Models.DTOs.RentalCenter;
using CarRentalApp.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Controllers
{
    [ApiController]
    [Route("api/rental-centers")]
    public class RentalCentersController : ControllerBase
    {
        private readonly RentalCenterService _rentalCenterService;
        private readonly CarService _carService;

        public RentalCentersController(RentalCenterService rentalCenterService, CarService carService)
        {
            _rentalCenterService = rentalCenterService;
            _carService = carService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalCenterDTO>>> GetCenters()
        {
            var result = await _rentalCenterService.GetCenterModelsAsync();

            var response = result.Select(
                center => center.Adapt<RentalCenterDTO>()
            );

            return Ok(response);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<RentalCenterDTO>> GetCenter(string name)
        {
            var result = await _rentalCenterService.GetCenterModelAsync(name);
            var response = result.Adapt<RentalCenterDTO>();
            return Ok(response);
        }

        [HttpPost("filtered")]
        public async Task<ActionResult<IEnumerable<RentalCenterDTO>>> GetFilteredCenters(CenterFilterRequest request)
        {
            var result = await _rentalCenterService.GetFilteredCenterModelsAsync(request);

            var response = result.Select(
                center => center.Adapt<RentalCenterDTO>()
            );

            return Ok(response);
        }

        [HttpGet("{name}/cars")]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetCenterCars(string name)
        {
            var result = await _rentalCenterService.GetCenterCarModelsAsync(name);

            var response = result.Select(
                car => car.Adapt<CarDTO>()
            );

            return Ok(response);
        }
    }
}