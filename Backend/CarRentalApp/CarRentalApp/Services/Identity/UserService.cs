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

        /// <exception cref="SharedException">Incorrect username or password.</exception>
        public void ValidateClient(User user, UserLoginDTO userLoginDTO)
        {
            if (!user.Roles.Intersect(User.ClientRoles).Any())
            {
                throw new SharedException(
                    ErrorTypes.AccessDenied,
                    "Wait for your account to be approved",
                    "User does not have client role"
                );
            }

            ValidatePassword(user, userLoginDTO);
        }

        /// <exception cref="SharedException">Incorrect username or password.</exception>
        public void ValidateAdmin(User user, UserLoginDTO userLoginDTO)
        {
            if (!user.Roles.Intersect(User.AdminRoles).Any())
            {
                throw new SharedException(
                    ErrorTypes.AccessDenied,
                    "User does not have permission",
                    "User does not have admin role"
                );
            }

            ValidatePassword(user, userLoginDTO);
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

        public Task UpdateRolesAsync(User user, UserDTO userDTO)
        {
            if (userDTO.Roles.Count < 1)
            {
                throw new SharedException(
                    ErrorTypes.Invalid,
                    "Role changing failed",
                    "User must have at least 1 role"
                );
            }

            if (user.Roles.SequenceEqual(userDTO.Roles))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Role changing failed",
                    "User already has specified roles"
                );
            }

            if (user.Roles.Intersect(User.ClientRoles).Any())
            {
                ValidateAge(user.DateOfBirth);

                if (user.DriverLicenseSerialNumber == null)
                {
                    throw new SharedException(
                        ErrorTypes.Conflict,
                        "Role changing failed",
                        "User cannot become a client without specifying DriverLicenseSerialNumber"
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

            user.Roles = new List<Roles>() {Roles.None};

            return user;
        }

        private void ValidateAge(DateTime dateOfBirth)
        {
            var minimumAge = _clientRequirements.MinimumAge;
            var criticalDate = DateTime.Now.AddYears(minimumAge);

            if (criticalDate.CompareTo(dateOfBirth) > 0)
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Client`s data does not meet requirements",
                    $"Client`s age must be greater than {minimumAge - 1}"
                );
            }
        }

        /// <exception cref="SharedException">Incorrect username or password.</exception>
        private void ValidatePassword(User existingUser, UserLoginDTO userLoginDTO)
        {
            if (!_passwordService.VerifyPassword(
                    existingUser.HashedPassword,
                    existingUser.Salt,
                    userLoginDTO.Password
                ))
            {
                throw new SharedException(
                    ErrorTypes.AuthFailed,
                    "Incorrect username or password",
                    "Incorrect password"
                );
            }
        }
    }
}