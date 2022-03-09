using CarRentalBll.Models;
using CarRentalBll.Models.RentalCenters;
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

            TypeAdapterConfig<Order, OrderModel>
                .NewConfig()
                .Map(
                    dest => dest.CarServices,
                    src => src.OrderCarServices.Select(orderService => orderService.CarService.Adapt<CarServiceModel>())
                )
                .Map(
                    dest => dest.Client,
                    src => src.Client.Adapt<UserModel>()
                )
                .Map(
                    dest => dest.RentalCenter,
                    src => src.RentalCenter.Adapt<RentalCenterModel>()
                );

            // TypeAdapterConfig<OrderModel, Order>
            //     .NewConfig()
            //     .Map(
            //         dest => dest.CarServices,
            //         src => src.CarServices.Select(service => service.Adapt<CarServiceModel>())
            //     )
            //     .Map(
            //         dest => dest.Client,
            //         src => src.Client.Adapt<UserModel>()
            //     )
            //     .Map(
            //         dest => dest.RentalCenter,
            //         src => src.RentalCenter.Adapt<RentalCenterModel>()
            //     );
        }
    }
}