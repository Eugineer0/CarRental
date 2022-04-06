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
        private readonly UserService _userService;

        public OrderService(
            CarRentalDbContext carRentalDbContext,
            CarService carService,
            IOptions<UserRequirements> userRequirements,
            UserService userService
        )
        {
            _carRentalDbContext = carRentalDbContext;
            _carService = carService;
            _userService = userService;
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
                .ThenInclude(orderCarServices => orderCarServices.CarService)
                .Include(order => order.Car)
                .ThenInclude(car => car.Type)
                .ThenInclude(carType => carType.CarServicePrices)
                .Include(order => order.Car)
                .ThenInclude(car => car.RentalCenter)
                .Select(order => order.Adapt<OrderModel>());
        }

        public async Task CreateAsync(OrderRequestModel orderRequestModel, string username)
        {
            var car = await ValidateCarAsync(orderRequestModel);

            ValidateRentalPeriod(orderRequestModel.StartRent, orderRequestModel.FinishRent);

            var orderCarServicesPrices = await ValidateServicesAsync(orderRequestModel.OrderCarServicesId, car);

            if (orderRequestModel.OverallPrice != GetOverallPriceAsync(orderRequestModel, orderCarServicesPrices, car))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Invalid price",
                    "Received price does not match locally calculated"
                );
            }

            var client = await _userService.GetUserByAsync(username);

            var order = await InsertOrderAsync(client, car, orderRequestModel);

            await InsertOrderCarServices(order, orderCarServicesPrices);
        }

        private async Task InsertOrderCarServices(Order order, ICollection<CarServicePrice> orderCarServicesPrices)
        {
            var orderCarServices = orderCarServicesPrices.Select(
                carServicePrice => new OrderCarService()
                {
                    CarServiceId = carServicePrice.CarServiceId,
                    Price = carServicePrice.Price,
                    OrderId = order.Id
                }
            );

            _carRentalDbContext.OrderCarServices.AddRange(orderCarServices);
            await _carRentalDbContext.SaveChangesAsync();
        }

        private async Task<Order> InsertOrderAsync(User client, Car car, OrderRequestModel orderRequestModel)
        {
            var order = orderRequestModel.Adapt<Order>();
            order.Car = car;
            order.Client = client;

            _carRentalDbContext.Orders.Add(order);
            await _carRentalDbContext.SaveChangesAsync();

            return order;
        }

        private async Task<ICollection<CarServicePrice>> ValidateServicesAsync(IEnumerable<int> orderCarServicesId, Car car)
        {
            var orderCarServicePrices = await _carRentalDbContext.CarServicePrices
                .Where(carServicePrice =>
                           carServicePrice.CarTypeId == car.TypeId
                           && orderCarServicesId.Contains(carServicePrice.CarServiceId)
                )
                .Include(carServicePrice => carServicePrice.CarService)
                .ToListAsync();

            if (orderCarServicePrices.Count != orderCarServicesId.Count())
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "Inconsistent services",
                    "Specified services are inapplicable to specified car"
                );
            }

            return orderCarServicePrices;
        }

        private void ValidateRentalPeriod(DateTime startRent, DateTime finishRent)
        {
            if (startRent.HasDurationUntil(
                    finishRent,
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
        }

        private async Task<Car> ValidateCarAsync(OrderRequestModel orderRequestModel)
        {
            var car = await _carService.GetByAsync(orderRequestModel.CarId);

            if (car.RentalCenterId != orderRequestModel.RentalCenterId)
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Invalid rental center",
                    "Car does not belongs to specified rental center"
                );
            }

            if (!_carService.CheckIfAvailable(car, orderRequestModel.StartRent, orderRequestModel.FinishRent))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Unavailable car",
                    "Car have been already booked"
                );
            }

            return car;
        }

        private decimal GetOverallPriceAsync(OrderRequestModel orderModel, IEnumerable<CarServicePrice> carServicePrices, Car car)
        {
            var carServicesPrice = carServicePrices.Aggregate(
                decimal.Zero,
                (result, service) => result + service.Price
            );

            var rentalPrice = _carService.GetRentalPrice(
                car,
                orderModel.StartRent,
                orderModel.FinishRent
            );

            return carServicesPrice + rentalPrice;
        }
    }
}