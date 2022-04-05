using CarRentalBll.Models;
using CarRentalWeb.Models.Responses;
using Mapster;

namespace CarRentalWeb.Configurations
{
    public static class MapsterWebConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<OrderModel, OrderResponse>
                .NewConfig()
                .Map(
                    dest => dest.OrderCarServices,
                    src => src.OrderCarServices.Select(service => service.Adapt<CarServiceResponse>())
                )
                .Map(
                    dest => dest.Car,
                    src => src.Car.Adapt<CarResponse>()
                )
                .Map(
                    dest => dest.RentalCenter,
                    src => src.RentalCenter.Adapt<RentalCenterResponse>()
                );

            TypeAdapterConfig<CarModel, CarResponse>
                .NewConfig()
                .Map(
                    dest => dest,
                    src => src.CarType.Adapt<CarResponse>()
                );
        }
    }
}