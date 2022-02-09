using AutoMapper;
using CarRentalApp.Configuration;
using CarRentalApp.Exceptions;
using CarRentalApp.Models.DAOs;
using CarRentalApp.Models.DTOs;
using CarRentalApp.Models.Entities;
using CarRentalApp.Services.Authentication;
using Microsoft.Extensions.Options;

namespace CarRentalApp.Services.Identity
{
    public class UserService
    {
        private readonly UserDAO _userDAO;
        private readonly PasswordService _passwordService;
        private readonly ClientRequirements _clientRequirements;

        private readonly IMapper _userMapper;

        public UserService(
            UserDAO userDAO,
            PasswordService passwordService,
            IMapper userMapper,
            IOptions<ClientRequirements> clientRequirements
        )
        {
            _userDAO = userDAO;
            _passwordService = passwordService;
            _userMapper = userMapper;
            _clientRequirements = clientRequirements.Value;
        }

        /// <exception cref="SharedException">User not found by <paramref name="userDTO"/>.</exception>
        public async Task<User> GetExistingUserAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _userDAO.GetByUsernameAsync(userLoginDTO.Username);
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

        /// <exception cref="SharedException">User not found by <paramref name="userDTO"/>.</exception>
        public async Task<User> GetExistingUserAsync(UserDTO userDTO)
        {
            var user = await _userDAO.GetByUsernameAsync(userDTO.Username);
            if (user == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "User not found",
                    "User with such username not found"
                );
            }

            return user;
        }

        public bool CheckIfPasswordValid(User existingUser, UserLoginDTO userLoginDTO)
        {
            return _passwordService.VerifyPassword(
                existingUser.HashedPassword,
                existingUser.Salt,
                userLoginDTO.Password
            );
        }

        /// <exception cref="SharedException"><paramref name="token"/> subject not found.</exception>
        public async Task<User> GetUserByRefreshTokenAsync(RefreshToken token)
        {
            var user = await _userDAO.GetByIdAsync(token.UserId);
            if (user == null)
            {
                throw new SharedException(
                    ErrorTypes.AuthFailed,
                    "Invalid token",
                    "User not found by refresh token userId"
                );
            }

            return user;
        }

        public async Task<User> RegisterAsync(UserRegistrationDTO userRegistrationDTO)
        {
            var user = CreateFromDTO(userRegistrationDTO);

            await _userDAO.InsertUserAsync(user);

            return user;
        }

        public Task UpgradeRoleAsync(User user)
        {
            switch (user.Roles)
            {
                case Roles.None:
                {
                    user.Roles = Roles.Admin;
                    break;
                }
                case Roles.Client:
                {
                    user.Roles = Roles.Admin;
                    break;
                }
                case Roles.Admin:
                {
                    user.Roles = Roles.SuperAdmin;
                    break;
                }
                case Roles.SuperAdmin:
                {
                    throw new SharedException(
                        ErrorTypes.Conflict,
                        "Role upgrading failed",
                        "User already has highest role"
                    );
                }
            }

            return _userDAO.UpdateUserAsync(user);
        }

        public Task DowngradeRoleAsync(User user)
        {
            switch (user.Roles)
            {
                case Roles.Admin:
                {
                    user.Roles = user.DriverLicenseSerialNumber == null ? Roles.None : Roles.Client;
                    break;
                }
                case Roles.SuperAdmin:
                {
                    user.Roles = Roles.Admin;
                    break;
                }
                default:
                {
                    throw new SharedException(
                        ErrorTypes.Conflict,
                        "Role downgrading failed",
                        "User already has lowest role"
                    );
                }
            }

            return _userDAO.UpdateUserAsync(user);
        }

        public Task<bool> CheckIfExistsAsync(UserRegistrationDTO userRegistrationDTO)
        {
            return _userDAO.CheckUniquenessAsync(userRegistrationDTO.Username, userRegistrationDTO.Email);
        }

        private User CreateFromDTO(UserRegistrationDTO userRegistrationDTO)
        {
            var user = _userMapper.Map<UserRegistrationDTO, User>(userRegistrationDTO);

            user.Salt = _passwordService.GenerateSalt();
            user.HashedPassword = _passwordService.DigestPassword(userRegistrationDTO.Password, user.Salt);

            if (userRegistrationDTO.DriverLicenseSerialNumber != null)
            {
                var minimumAge = _clientRequirements.MinimumAge;
                var criticalDate = DateTime.Now.AddYears(minimumAge);

                if (criticalDate.CompareTo(userRegistrationDTO.DateOfBirth) > 0)
                {
                    throw new SharedException(
                        ErrorTypes.Conflict,
                        "Client`s data does not meet requirements",
                        $"Client`s age must be greater than {minimumAge - 1}"
                    );
                }

                user.Roles = Roles.Client;
            }
            else
            {
                user.Roles = Roles.None;
            }

            return user;
        }
    }
}