using CarRentalApp.Models.Dal;
using CarRentalBll.Models;
using Mapster;

namespace CarRentalBll.Configuration.Mappers
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