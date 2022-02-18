using AutoMapper;
using CarRentalApp.Configuration;
using CarRentalApp.Contexts;
using CarRentalApp.Exceptions;
using CarRentalApp.Models.DTOs;
using CarRentalApp.Models.DTOs.Registration;
using CarRentalApp.Models.Entities;
using CarRentalApp.Services.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarRentalApp.Services.Identity
{
    public class UserService
    {
        private readonly PasswordService _passwordService;
        private readonly IMapper _userMapper;
        private readonly ClientRequirements _clientRequirements;
        private readonly CarRentalDbContext _carRentalDbContext;

        public UserService(
            PasswordService passwordService,
            IMapper userMapper,
            IOptions<ClientRequirements> clientRequirements, CarRentalDbContext carRentalDbContext)
        {
            _passwordService = passwordService;
            _userMapper = userMapper;
            _carRentalDbContext = carRentalDbContext;
            _clientRequirements = clientRequirements.Value;
        }

        /// <exception cref="SharedException">User not found by <paramref name="userId"/>.</exception>
        public async Task CompleteRegistrationAsync(Guid userId, CompleteRegistrationDTO completeRegistrationDTO)
        {
            var user = await _carRentalDbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "User not found",
                    "User with such username not found"
                );
            }

            user.DriverLicenseSerialNumber = completeRegistrationDTO.DriverLicenseSerialNumber;
            _carRentalDbContext.Users.Add(user);

            await _carRentalDbContext.SaveChangesAsync();
        }

        /// <exception cref="SharedException">User with such credentials already exists.</exception>
        public async Task RegisterAsync(UserRegistrationDTO userRegistrationDTO)
        {
            if (await _carRentalDbContext.Users
                    .AnyAsync(
                        u => u.Username.Equals(userRegistrationDTO.Username)
                             || u.Email.Equals(userRegistrationDTO.Email)
                    )
               )
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "User with such email or username already exists"
                );
            }

            var user = CreateFromDTO(userRegistrationDTO);
            _carRentalDbContext.Users.Add(user);

            await _carRentalDbContext.SaveChangesAsync();
        }

        /// <exception cref="SharedException">User not found by <paramref name="userInfo"/> username.</exception>
        public async Task<User> GetUserAsync(string username)
        {
            var user = await _carRentalDbContext.Users
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Username.Equals(username));
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

        /// <exception cref="SharedException"><paramref name="token"/> subject not found.</exception>
        public async Task<User> GetByRefreshTokenAsync(RefreshToken token)
        {
            var user = await _carRentalDbContext.Users.FirstOrDefaultAsync(user => user.Id == token.Id);
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

        public Task UpdateRolesAsync(User user, RolesDTO rolesDTO)
        {
            if (user.Roles.Select(role => role.Role).SequenceEqual(rolesDTO.Roles))
            {
                return Task.CompletedTask;
            }

            user.Roles = rolesDTO.Roles.Select(role => new UserRole() {Role = role}).ToList();

            _carRentalDbContext.Users.Update(user);

            return _carRentalDbContext.SaveChangesAsync();
        }
        
        /// <exception cref="SharedException">Client role is not approved.</exception>
        public bool Validate(User user, UserLoginDTO userLoginDTO)
        {
            ValidatePassword(user.HashedPassword, user.Salt, userLoginDTO.Password);

            if (user.Roles.Select(role => role.Role).Intersect(UserRole.ClientRoles).Any())
            {
                return true;
            }

            if (user.DriverLicenseSerialNumber == null)
            {
                return false;
            }

            throw new SharedException(
                ErrorTypes.AccessDenied,
                "Wait for your account to be approved",
                "User does not have client role"
            );
        }

        /// <exception cref="SharedException">Inconsistent user role.</exception>
        public void ValidateAdmin(User user, UserLoginDTO userLoginDTO)
        {
            ValidatePassword(user.HashedPassword, user.Salt, userLoginDTO.Password);

            if (!user.Roles.Select(role => role.Role).Intersect(UserRole.AdminRoles).Any())
            {
                throw new SharedException(
                    ErrorTypes.AccessDenied,
                    "You do not have permission",
                    "User does not have admin role"
                );
            }
        }


        /// <exception cref="SharedException">Empty roles set.</exception>
        /// <exception cref="SharedException">Cannot specify client role without additional info.</exception>
        public void ValidateNewRoles(User user, RolesDTO rolesDTO)
        {
            if (rolesDTO.Roles.Count < 1)
            {
                throw new SharedException(
                    ErrorTypes.Invalid,
                    "Role changing failed",
                    "User must have at least 1 role"
                );
            }

            if (rolesDTO.Roles.Intersect(UserRole.ClientRoles).Any())
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
        }

        private User CreateFromDTO(UserRegistrationDTO userRegistrationDTO)
        {
            var user = _userMapper.Map<UserRegistrationDTO, User>(userRegistrationDTO);

            user.Salt = _passwordService.GenerateSalt();
            user.HashedPassword = _passwordService.DigestPassword(userRegistrationDTO.Password, user.Salt);

            user.Roles = new List<UserRole>() {new UserRole() {Role = Roles.None}};

            return user;
        }

        public static bool CheckMinimumAge(DateTime dateOfBirth, int minimumAge)
        {
            var criticalDate = dateOfBirth.AddYears(minimumAge);

            return DateTime.Now.CompareTo(criticalDate) > 0;
        }

        /// <exception cref="SharedException">Invalid user age.</exception>
        private void ValidateAge(DateTime dateOfBirth)
        {
            var minimumAge = _clientRequirements.MinimumAge;

            if (CheckMinimumAge(dateOfBirth, minimumAge))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Client`s data does not meet requirements",
                    $"Client`s age must be greater than {minimumAge - 1}"
                );
            }
        }

        /// <exception cref="SharedException">Incorrect username or password.</exception>
        private void ValidatePassword(byte[] hashedPassword, byte[] salt, string password)
        {
            if (_passwordService.VerifyPassword(hashedPassword, salt, password))
            {
                return;
            }

            throw new SharedException(
                ErrorTypes.AuthFailed,
                "Incorrect username or password",
                "Incorrect password"
            );
        }
    }
}