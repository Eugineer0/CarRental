using CarRentalApp.Models.DTOs.Car;
using CarRentalApp.Models.DTOs.RentalCenter;
using CarRentalApp.Services;
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

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<RentalCenterDTO>>> GetCenters()
        // {
        //     return Ok(await _rentalCenterService.GetAllCenters());
        // }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalCenterDTO>>> GetCenters(DateTime start, DateTime finish)
        {
            var centers = await _rentalCenterService.GetCentersFilteredByDate(start, finish);

            return Ok(centers);
        }

        [HttpGet("{name}/cars")]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetCenterAccessibleCars(
            string name,
            DateTime start,
            DateTime finish
        )
        {
            var rentalCenter = await _rentalCenterService.GetCenterAsync(name);

            return Ok(await _carService.GetAccessibleCars(rentalCenter, start, finish));
        }
    }
}