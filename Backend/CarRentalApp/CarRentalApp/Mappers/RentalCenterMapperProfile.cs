using AutoMapper;
using CarRentalApp.Models.DTOs;
using CarRentalApp.Models.DTOs.Car;
using CarRentalApp.Models.DTOs.Registration;
using CarRentalApp.Models.DTOs.RentalCenter;
using CarRentalApp.Models.Entities;

namespace CarRentalApp.Mappers
{
    public class RentalCenterMapperProfile : Profile
    {
        public RentalCenterMapperProfile()
        {
            CreateMap<RentalCenterDTO, RentalCenter>();
            CreateMap<RentalCenter, RentalCenterDTO>();
        }
    }
}