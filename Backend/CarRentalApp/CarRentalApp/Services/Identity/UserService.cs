using AutoMapper;
using CarRentalApp.Exceptions;
using CarRentalApp.Models.DTOs;
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

        /// <exception cref="SharedException">User not found by <paramref name="userDTO"/>.</exception>
        public async Task<User> GetExistingUserAsync(UserLoginDTO userDTO)
        {
            var user = await _userRepository.GetByUsernameAsync(userDTO.Username);
            if (user == null)
            {
                throw new SharedException(
                    ErrorTypes.AuthFailed,
                    "Incorrect username or password",
                    "User with such username not found"
                );
            }

            return user;
        }

        /// <exception cref="SharedException">User not found by <paramref name="adminDTO"/>.</exception>
        public async Task<User> GetExistingUserAsync(AdminAssignmentDTO adminDTO)
        {
            var user = await _userRepository.GetByUsernameAsync(adminDTO.Username);
            if (user == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "User with such username not found"
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

        /// <exception cref="SharedException"><paramref name="token"/> subject not found.</exception>
        public async Task<User> GetUserByRefreshTokenAsync(RefreshToken token)
        {
            var user = await _userRepository.GetByIdAsync(token.UserId);
            if (user == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "User not found"
                );
            }

            return user;
        }

        public async Task<User> RegisterAsync(UserRegistrationDTO userDTO)
        {
            var user = CreateFromDTO(userDTO);

            await _userRepository.InsertUserAsync(user);

            return user;
        }

        public async Task AssignAdminAsync(User user)
        {
            switch (user.Role)
            {
                case Role.None:
                {
                    break;
                }
                case Role.Client:
                {
                    throw new SharedException(
                        ErrorTypes.Invalid,
                        "Admin assignment failed",
                        "Cannot assign client to admin"
                    );
                }
                case Role.Admin:
                {
                    throw new SharedException(
                        ErrorTypes.Invalid,
                        "Admin assignment failed",
                        "User already has admin role"
                    );
                }
                case Role.SuperAdmin:
                {
                    throw new SharedException(
                        ErrorTypes.Invalid,
                        "Admin assignment failed",
                        "Cannot downgrade super-admin"
                    );
                }
            }

            user.Role = Role.Admin;
            await _userRepository.UpdateUserAsync(user);
        }

        public Task<bool> CheckIfExistsAsync(UserRegistrationDTO userDTO)
        {
            return _userRepository.CheckUniquenessAsync(userDTO.Username, userDTO.Email);
        }

        private User CreateFromDTO(UserRegistrationDTO userDTO)
        {
            var user = _userMapper.Map<UserRegistrationDTO, User>(userDTO);

            user.Salt = _passwordService.GenerateSalt();
            user.HashedPassword = _passwordService.DigestPassword(userDTO.Password, user.Salt);
            user.Role = Role.None;

            return user;
        }
    }
}