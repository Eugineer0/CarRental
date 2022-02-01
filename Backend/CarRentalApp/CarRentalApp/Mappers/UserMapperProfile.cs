using AutoMapper;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.Entities;
using CarRentalApp.Services.Authentication;

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