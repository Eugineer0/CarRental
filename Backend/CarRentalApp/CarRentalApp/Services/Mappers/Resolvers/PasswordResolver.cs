using AutoMapper;
using CarRentalApp.Models.Entities;
using CarRentalApp.Models.Requests.DTOs;

namespace CarRentalApp.Services.Authentication
{
    public class PasswordResolver : IValueResolver<UserRegistrationDTO, User, byte[]>
    {
        private readonly PasswordService _passwordService;

        public PasswordResolver(PasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public byte[] Resolve(UserRegistrationDTO source, User destination, byte[] destMember,
            ResolutionContext context)
        {
            return _passwordService.DigestPassword(source.Password, destination.Salt);
        }
    }
}