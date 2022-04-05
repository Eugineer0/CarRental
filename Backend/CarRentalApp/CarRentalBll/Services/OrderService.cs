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
                .ThenInclude(client => client.Roles)
                .Where(order => order.Client.Username == username)
                .Include(order => order.OrderCarServices)
                .Include(order => order.Car)
                .ThenInclude(car => car.Type)
                .ThenInclude(carType => carType.CarServicePrices)
                .ThenInclude(carTypeServicePrice => carTypeServicePrice.CarService)
                .Include(order => order.Car)
                .ThenInclude(car => car.RentalCenter)
                .Include(order => order.Car)
                .ThenInclude(car => car.Type)
                .Select(order => order.Adapt<OrderModel>());
        }

        // private OrderModel Convert(Order order)
        // {
        //     var orderModel = order.Adapt<OrderModel>();
        //     foreach (var orderService in order.OrderCarServices)
        //     {
        //         orderModel.OrderCarServices
        //     }
        //     orderModel.OrderCarServices.Select(orderService => orderService.Price) = order.OrderCarServices.Select(orderCarService => orderCarService.Service.Prices.Where);
        // }

        private decimal GetOrderServicePrice(Order order, CarRentalDal.Models.CarService orderCarService)
        {
            var service = orderCarService.Prices
                .FirstOrDefault(price => price.CarTypeId == order.Car.Type.Id);

            if (service == null)
            {
                throw new SharedException(
                    ErrorTypes.Invalid,
                    "Inconsistent order services for specified car"
                );
            }

            return service.Price;
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
                .Select(carService => carService.Id);

            if (!availableServices.ContainsAll(orderModel.OrderCarServices.Select(service => service.Id)))
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
                .Where(
                    servicePrice =>
                        servicePrice.CarTypeId == orderModel.Car.CarType.Id
                        && orderModel.OrderCarServices.Select(service => service.Id).Contains(servicePrice.Id)
                )
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