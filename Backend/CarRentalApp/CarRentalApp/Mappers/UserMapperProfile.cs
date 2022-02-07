using AutoMapper;
using CarRentalApp.Models.DTOs;
using CarRentalApp.Models.Entities;

namespace CarRentalApp.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserRegistrationDTO, User>();
        }
    }
}