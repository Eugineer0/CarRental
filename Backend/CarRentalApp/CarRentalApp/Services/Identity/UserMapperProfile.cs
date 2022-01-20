using AutoMapper;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.Entities;
using CarRentalApp.Services.Authentication;

namespace CarRentalApp.Services.Identity
{
    public class UserMapperProfile : Profile
    {            
        public UserMapperProfile()
        {   
            var passwordService = new PasswordService();

            byte[] salt = passwordService.GenerateSalt();

            CreateMap<UserRegistrationDTO, User>()
                .ForMember(
                    dst => dst.Salt,
                    opt => opt.MapFrom(_ => salt)
                )
                .ForMember(
                    dst => dst.HashedPassword,
                    opt => opt.MapFrom(
                        src => passwordService.DigestPassword(src.Password, salt)
                    )
                )                    
                .ForMember(
                    dst => dst.Role,
                    opt => opt.MapFrom(_ => Role.User)
                );           
        }
    }
}
