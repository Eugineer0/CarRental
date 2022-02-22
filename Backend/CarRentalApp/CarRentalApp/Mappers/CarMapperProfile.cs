using AutoMapper;
using CarRentalApp.Models.DTOs;
using CarRentalApp.Models.DTOs.Car;
using CarRentalApp.Models.DTOs.Registration;
using CarRentalApp.Models.Entities;

namespace CarRentalApp.Mappers
{
    public class CarMapperProfile : Profile
    {
        public CarMapperProfile()
        {
            CreateMap<CarDTO, Car>();
            CreateMap<Car, CarDTO>();
        }
    }
}