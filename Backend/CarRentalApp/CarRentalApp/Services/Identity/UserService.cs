using AutoMapper;
using CarRentalApp.Exceptions.BLL;
using CarRentalApp.Models.DTOs.Requests;
using CarRentalApp.Models.Entities;
using CarRentalApp.Repositories;
using CarRentalApp.Services.Authentication;

namespace CarRentalApp.Services.Identity
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordService _passwordService;

        private readonly IMapper _userMapper;

        public UserService(
            UserRepository userRepository,
            PasswordService passwordService,
            IMapper userMapper
        )
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _userMapper = userMapper;
        }

        public async Task<User> GetExistingUserAsync(UserLoginDTO userDTO)
        {
            var user = await _userRepository.GetByUsernameAsync(userDTO.Username);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return user;
        }

        public bool IsValid(User existingUser, UserLoginDTO userDTO)
        {
            return _passwordService.VerifyPassword(
                existingUser.HashedPassword,
                existingUser.Salt,
                userDTO.Password
            );
        }

        public async Task<User> GetByRefreshTokenAsync(RefreshToken token)
        {
            var user = await _userRepository.GetByIdAsync(token.UserId);
            if (user == null)
            {
                throw new TokenOwnerNotFoundException();
            }

            return user;
        }

        public async Task<User> RegisterAsync(UserRegistrationDTO userDTO, bool isAdmin)
        {
            var user = CreateFromDTO(userDTO, isAdmin);

            await _userRepository.InsertUserAsync(user);

            return user;
        }

        public Task<bool> CheckIfExistsAsync(UserRegistrationDTO userDTO)
        {
            return _userRepository.AreUniqueCredentialsAsync(userDTO.Username, userDTO.Email);
        }

        private User CreateFromDTO(UserRegistrationDTO userDTO, bool isAdmin)
        {
            var user = _userMapper.Map<UserRegistrationDTO, User>(userDTO);

            user.Salt = _passwordService.GenerateSalt();
            user.HashedPassword = _passwordService.DigestPassword(userDTO.Password, user.Salt);
            user.Role = isAdmin ? Role.Admin : Role.User;

            return user;
        }
    }
}