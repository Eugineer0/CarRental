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
        }
    }
}