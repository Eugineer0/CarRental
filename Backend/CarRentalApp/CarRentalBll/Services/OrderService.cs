using CarRentalBll.Models;
using CarRentalDal.Contexts;
using CarRentalDal.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedResources.Configurations;
using SharedResources.Exceptions;
using SharedResources.Helpers;

namespace CarRentalBll.Services
{
    public class OrderService
    {
        private readonly CarRentalDbContext _carRentalDbContext;
        private readonly CarService _carService;
        private readonly UserRequirements _userRequirements;

        public OrderService(
            CarRentalDbContext carRentalDbContext,
            CarService carService,
            IOptions<UserRequirements> userRequirements
        )
        {
            _carRentalDbContext = carRentalDbContext;
            _carService = carService;
            _userRequirements = userRequirements.Value;
        }

        /// <summary>
        /// Returns all orders of user with specified <paramref name="userId"/> .
        /// </summary>
        /// <param name="userId">unique credential of user.</param>
        /// <returns>Existing user orders models.</returns>
        public IQueryable<OrderModel> GetOrdersBy(Guid userId)
        {
            return _carRentalDbContext.Orders
                .Where(order => order.ClientId == userId)
                .Include(order => order.OrderCarServices)
                .Include(order => order.Car)
                .Select(order => order.Adapt<OrderModel>());
        }

        /// <summary>
        /// Returns all orders of user with specified <paramref name="username"/> .
        /// </summary>
        /// <param name="username">unique credential of user.</param>
        /// <returns>Existing user orders models.</returns>
        public IQueryable<OrderModel> GetOrdersBy(string username)
        {
            return _carRentalDbContext.Orders
                .Include(order => order.Client)
                .Where(order => order.Client.Username == username)
                .Include(order => order.OrderCarServices)
                .Include(order => order.Car)
                .Select(order => order.Adapt<OrderModel>());
        }

        public Task CreateAsync(OrderModel model)
        {
            var order = model.Adapt<Order>();
            _carRentalDbContext.Orders.Add(order);
            return _carRentalDbContext.SaveChangesAsync();
        }

        public async void ValidateAsync(OrderModel orderModel)
        {
            if (orderModel.Car.RentalCenterId != orderModel.RentalCenter.Id)
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Invalid rental center",
                    "Car does not belongs to specified rental center"
                );
            }

            if (orderModel.StartRent.CheckPeriodDuration(
                orderModel.FinishRent,
                _userRequirements.MinimumRentalPeriodDurationMinutes
                )
            )
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Invalid rental period",
                    $"Rental period must have duration more than {_userRequirements.MinimumRentalPeriodDurationMinutes} minutes"
                );
            }

            var availableServices = _carRentalDbContext.CarServicePrices
                .Where(carService => carService.CarTypeId == orderModel.Car.CarType.Id)
                .Include(carService => carService.CarService)
                .Select(carService => carService.CarService.Adapt<CarServiceModel>());

            if (!availableServices.ContainsAll(orderModel.CarServices))
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "Inconsistent services",
                    "Specified services are inapplicable to specified car"
                );
            }

            var car = await _carService.GetByAsync(orderModel.Car.Id);

            if (!_carService.CheckIfAvailable(car, orderModel.StartRent, orderModel.FinishRent))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Unavailable car",
                    "Car have been already booked"
                );
            }

            if (orderModel.OverallPrice != await GetOverallPriceAsync(orderModel))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Invalid price",
                    "Received price does not match locally calculated"
                );
            }
        }

        private async Task<decimal> GetOverallPriceAsync(OrderModel orderModel)
        {
            var servicePrices = _carRentalDbContext.CarServicePrices
                .Where(service => orderModel.CarServices.Contains(service.Adapt<CarServiceModel>()))
                .Aggregate(Decimal.Zero, (result, service) => result + service.Price);

            var rentalPrice = await _carService.GetRentalPriceAsync(
                orderModel.Car.Id,
                orderModel.StartRent,
                orderModel.FinishRent
            );

            return servicePrices + rentalPrice;
        }
    }
}