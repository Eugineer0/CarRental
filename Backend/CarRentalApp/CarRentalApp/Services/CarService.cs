﻿using CarRentalApp.Contexts;
using CarRentalApp.Models.BLL;
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

        public IEnumerable<CarModel> GetCars(IEnumerable<Car> cars)
        {
            return cars.Select(car => car.Adapt<CarModel>());
        }

        public async Task<IEnumerable<CarModel>> GetAccessibleCars(IEnumerable<Car> cars, DateTime startRent, DateTime finishRent)
        {
            return cars.Where(car => CheckIfAvailable(car, startRent, finishRent))
                .Select(car => car.Adapt<CarModel>());
        }

        public bool CheckIfAvailable(Car car, DateTime start, DateTime finish)
        {
            return car.Orders
                .Select(order => new { order.FinishRent, order.StartRent })
                .All(
                    date =>
                        CheckIfAfter(date.FinishRent, start)
                        && CheckIfAfter(date.StartRent, finish)
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