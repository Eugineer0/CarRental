using CarRentalBll.Models;
using CarRentalDal.Models;
using Mapster;
using CarService = CarRentalBll.Services.CarService;

namespace CarRentalBll.Configurations
{
    public static class MapsterBllConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<User, UserModel>
                .NewConfig()
                .Map(dest => dest.Roles, src => src.Roles.Select(role => role.Role));

            TypeAdapterConfig<Order, OrderModel>
                .NewConfig()
                .Map(
                    dst => dst.ClientId,
                    src => src.Client.Id
                )
                .Map(
                    dest => dest.OrderCarServices,
                    src => src.OrderCarServices.Select(orderService => orderService.Adapt<CarServiceModel>())
                )
                .Map(
                    dest => dest.RentalCenter,
                    src => src.Car.RentalCenter.Adapt<RentalCenterModel>()
                );

            TypeAdapterConfig<Car, CarModel>
                .NewConfig()
                .Map(
                    dest => dest.CarType,
                    src => src.Type.Adapt<CarTypeModel>()
                )
                .Map(
                    dest => dest.RentalCenterId,
                    src => src.RentalCenter.Id
                );

            TypeAdapterConfig<CarType, CarTypeModel>
                .NewConfig()
                .Map(
                    dest => dest.AvailableServices,
                    src => src.CarServicePrices.Select(carServicePrice => carServicePrice.Adapt<CarServiceModel>())
                );

            TypeAdapterConfig<OrderCarService, CarServiceModel>
                .NewConfig()
                .Map(
                    dst => dst.Name,
                    src => src.CarService.Name
                );

            TypeAdapterConfig<CarServicePrice, CarServiceModel>
                .NewConfig()
                .Map(
                    dst => dst.Name,
                    src => src.CarService.Name
                );
        }
    }
}