using AutoMapper;
using CarRentalApp.Exceptions;
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

        /// <exception cref="GeneralException">User not found by <paramref name="userDTO"/>.</exception>
        public async Task<User> GetExistingUserAsync(UserLoginDTO userDTO)
        {
            var user = await _userRepository.GetByUsernameAsync(userDTO.Username);
            if (user == null)
            {
                throw new GeneralException(
                    GeneralException.ErrorTypes.NotFound,
                    "User not found",
                    null
                );
            }

            return user;
        }

        public bool CheckIfValid(User existingUser, UserLoginDTO userDTO)
        {
            return _passwordService.VerifyPassword(
                existingUser.HashedPassword,
                existingUser.Salt,
                userDTO.Password
            );
        }

        /// <exception cref="GeneralException"><paramref name="token"/> subject not found.</exception>
        public async Task<User> GetUserByRefreshTokenAsync(RefreshToken token)
        {
            var user = await _userRepository.GetByIdAsync(token.UserId);
            if (user == null)
            {
                throw new GeneralException(
                    GeneralException.ErrorTypes.NotFound,
                    "User not found",
                    null
                );
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
            return _userRepository.CheckUniquenessAsync(userDTO.Username, userDTO.Email);
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