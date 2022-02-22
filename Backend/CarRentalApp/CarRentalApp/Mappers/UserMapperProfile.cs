﻿using AutoMapper;
using CarRentalApp.Models.DTOs;
using CarRentalApp.Models.DTOs.Registration;
using CarRentalApp.Models.Entities;

namespace CarRentalApp.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserRegistrationDTO, User>();
            CreateMap<User, FullUserDTO>();
        }
    }
}