using CarRentalBll.Models;
using CarRentalDal.Models;
using Mapster;

namespace CarRentalBll.Configurations
{
    public static class MapsterBllConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<User, UserModel>
                .NewConfig()
                .Map(dest => dest.Roles, src => src.Roles.Select(role => role.Role));

            TypeAdapterConfig<User, UserModel>
                .NewConfig()
                .Map(dest => dest.Roles, src => src.Roles.Select(role => role.Role));

            TypeAdapterConfig<Order, OrderModel>
                .NewConfig()
                .Map(
                    dest => dest.OrderCarServices,
                    src => src.OrderCarServices.Select(orderService => orderService.CarService.Adapt<CarServiceModel>())
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
                    src => src.CarServicePrices.Select(sp => sp.CarService.Adapt<CarServiceModel>())
                );
        }
    }
}