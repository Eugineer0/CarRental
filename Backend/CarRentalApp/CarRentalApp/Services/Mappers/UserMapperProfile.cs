using AutoMapper;
using CarRentalApp.Models.Entities;
using CarRentalApp.Models.Requests.DTOs;
using CarRentalApp.Services.Authentication;

namespace CarRentalApp.Services.Identity
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserRegistrationDTO, User>()
                .ForMember(
                    dst => dst.Salt,
                    opt => opt.MapFrom<SaltResolver>()
                )
                .ForMember(
                    dst => dst.HashedPassword,
                    opt => opt.MapFrom<PasswordResolver>()
                );
        }
    }
}