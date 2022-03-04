using CarRentalApp.Models.Bll;
using CarRentalApp.Models.Dal;
using Mapster;

namespace CarRentalApp.Configuration.Mappers
{
    public class MapsterConfig
    {
        public static TypeAdapterConfig GetConfig()
        {
            var setter = TypeAdapterConfig<User, UserModel>
                .NewConfig()
                .Map(dest => dest.Roles, src => src.Roles.Select(role => role.Role));

            return setter.Config;
        }
    }
}