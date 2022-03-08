using CarRentalBll.Models;
using CarRentalDal.Contexts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SharedResources;

namespace CarRentalBll.Services
{
    public class OrderService
    {
        private readonly CarRentalDbContext _carRentalDbContext;
        private readonly CarService _carService;

        public OrderService(CarRentalDbContext carRentalDbContext, CarService carService)
        {
            _carRentalDbContext = carRentalDbContext;
            _carService = carService;
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
                .Include(order => order.CarServices)
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
                .Include(order => order.CarServices)
                .Include(order => order.Car)
                .Select(order => order.Adapt<OrderModel>());
        }

        public async void ValidateOrder(OrderModel orderModel)
        {
            if (orderModel.Car.RentalCenterId != orderModel.RentalCenter.Id)
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Invalid rental center",
                    "Car does not belongs to specified rental center"
                );
            }

            var availableServices = _carRentalDbContext.CarServicePrices
                .Where(carService => carService.CarTypeId == orderModel.Car.CarType.Id)
                .Include(carService => carService.CarService)
                .Select(carService => carService.CarService.Adapt<CarServiceModel>());

            if (availableServices.ContainsAll(orderModel.CarServices))
            {
                var car = await _carService.GetByAsync(orderModel.Car.Id);

                if (_carService.CheckIfAvailable(car, orderModel.StartRent, orderModel.FinishRent))
                {
                    return;
                }

                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Unavailable car",
                    "Car have been already booked"
                );
            }

            throw new SharedException(
                ErrorTypes.NotFound,
                "Inconsistent services",
                "Specified services are inapplicable to specified car"
            );
        }

        public void CreateOrderAsync(OrderModel model)
        {
            throw new NotImplementedException();
        }
    }
}