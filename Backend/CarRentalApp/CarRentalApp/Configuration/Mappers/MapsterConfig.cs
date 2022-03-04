using CarRentalApp.Models.BLL;
using CarRentalApp.Models.DTOs;
using CarRentalApp.Models.DTOs.RentalCenter;
using CarRentalApp.Models.Entities;
using Mapster;

namespace CarRentalApp.Configuration.Mappers
{
    public class MapsterConfig
    {
        public static TypeAdapterConfig GetConfig()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.NewConfig<User, FullUserDTO>()
                .Map(dest => dest.Roles, src => src.UserRoles.Select(role => role.Role));

            config.NewConfig<RentalCenterModel, RentalCenterDTO>()
                .Map(dest => dest.AvailableCarsNumber, src => src.Cars.Count);

            config.NewConfig<Car, CarModel>()
                .Map(dest => dest, src => src.Type.Adapt<CarModel>())
                .Map(dest => dest.AvailableServices, src => src.Type.ServicePrices.Select(sp => sp.Service.Adapt<ServiceModel>()));

            // config.NewConfig<CarModel, CarDTO>()
            //     .Map(dest => dest, src => src.Type.Adapt<CarTypeModel>());

            return config;
        }
    }
}