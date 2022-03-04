using CarRentalApp.Models.Bll;
using CarRentalApp.Models.Dal;
using Mapster;

namespace CarRentalApp.Configuration.Mappers
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<User, UserModel>
                .NewConfig()
                .Map(dest => dest.Roles, src => src.Roles.Select(role => role.Role));
        }
    }
}