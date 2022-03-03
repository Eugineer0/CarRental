using CarRentalApp.Models.Bll;
using CarRentalApp.Models.Dal;
using Mapster;
using MapsterMapper;

namespace CarRentalApp.Configuration.Mappers
{
    public class MapsterConfig
    {
        public static TypeAdapterConfig GetConfig()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.NewConfig<User, UserModel>()
                .Map(dest => dest.Roles, src => src.Roles.Select(role => role.Role));

            return config;
        }
    }
}