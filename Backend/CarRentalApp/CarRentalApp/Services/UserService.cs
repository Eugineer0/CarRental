using CarRentalApp.Configuration;
using CarRentalApp.Contexts;
using CarRentalApp.Exceptions;
using CarRentalApp.Models.Dto;
using CarRentalApp.Models.Dto.Registration;
using CarRentalApp.Models.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarRentalApp.Services
{
    public class UserService
    {
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;
        private readonly ClientRequirements _clientRequirements;
        private readonly CarRentalDbContext _carRentalDbContext;

        public UserService(
            PasswordService passwordService,
            IOptions<ClientRequirements> clientRequirements,
            CarRentalDbContext carRentalDbContext,
            TokenService tokenService
        )
        {
            _passwordService = passwordService;
            _carRentalDbContext = carRentalDbContext;
            _tokenService = tokenService;
            _clientRequirements = clientRequirements.Value;
        }

        /// <summary>
        /// Gets user by token and fills in driver license field
        /// </summary>
        /// <param name="completeRegistrationDto">pair of auth token and driver license serial number</param>
        public async Task CompleteRegistrationAsync(CompleteRegistrationDto completeRegistrationDto)
        {
            var userId = _tokenService.GetUserId(completeRegistrationDto.Token);
            var user = await GetByIdAsync(userId);

            user.DriverLicenseSerialNumber = completeRegistrationDto.DriverLicenseSerialNumber;

            await _carRentalDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Validates passed user prototype as client and returns generated access object for that user.
        /// </summary>
        /// <param name="userLoginDto">user prototype to login.</param>
        /// <returns>Generated access object.</returns>
        /// <exception cref="SharedException">User must specify driver license field</exception>
        public async Task<AuthenticationDto> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await GetByUsernameAsync(userLoginDto.Username);
            if (ValidateClient(user, userLoginDto))
            {
                return await _tokenService.GetAccess(user);
            }

            var token = _tokenService.GenerateRefreshToken(user);
            throw new SharedException(
                ErrorTypes.AdditionalDataRequired,
                token,
                "Registration completion required"
            );
        }

        /// <summary>
        /// Validates passed user prototype as admin and returns generated access object for that user.
        /// </summary>
        /// <param name="userLoginDto">user prototype to login.</param>
        /// <returns>Generated access object.</returns>
        public async Task<AuthenticationDto> LoginAdminAsync(UserLoginDto userLoginDto)
        {
            var user = await GetByUsernameAsync(userLoginDto.Username);
            ValidateAdmin(user, userLoginDto);
            return await _tokenService.GetAccess(user);
        }

        /// <summary>
        /// Checks uniqueness of credentials and saves created user model in database.
        /// </summary>
        /// <param name="userRegistrationDto">user prototype to register.</param>
        public async Task RegisterAsync(UserRegistrationDto userRegistrationDto)
        {
            await CheckCredentialsUniqueness(userRegistrationDto.Email, userRegistrationDto.Username);
            var user = CreateFromDto(userRegistrationDto);
            _carRentalDbContext.Users.Add(user);
            await _carRentalDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Validates passed token and replaces it with new refresh token.
        /// </summary>
        /// <param name="refreshTokenDto">token prototype to authenticate user.</param>
        /// <returns>Generated access object.</returns>
        public async Task<AuthenticationDto> RefreshAccessAsync(RefreshTokenDto refreshTokenDto)
        {
            var token = await _tokenService.PopTokenAsync(refreshTokenDto);

            _tokenService.ValidateTokenLifetime(token.Token);

            var user = await GetByIdAsync(token.UserId);
            return await _tokenService.GetAccess(user);
        }

        /// <summary>
        /// Updates user model found by passed username with client role.
        /// </summary>
        /// <param name="username">user model field.</param>
        /// <exception cref="SharedException">User with such <paramref name="username"/> does not meet requirements.</exception>
        public async Task ApproveClientAsync(string username)
        {
            var user = await GetByUsernameAsync(username);

            if (user.Roles.Select(role => role.Role).Intersect(UserRole.ClientRoles).Any())
            {
                return;
            }

            ValidateAge(user.DateOfBirth);

            if (user.DriverLicenseSerialNumber == null)
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Client approval failed",
                    "User cannot become a client without specifying DriverLicenseSerialNumber"
                );
            }

            user.Roles.Add(new UserRole() { Role = Roles.Client });
            await _carRentalDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Validates passed roles and assign them to existing user model found by username.
        /// </summary>
        /// <param name="username">user model field.</param>
        /// <param name="rolesDto">object, containing array of roles.</param>
        public async Task ModifyUserRolesAsync(string username, RolesDto rolesDto)
        {
            var user = await GetByUsernameAsync(username);
            ValidateNewRoles(user, rolesDto);
            await UpdateRolesAsync(user, rolesDto);
        }

        /// <summary>
        /// Checks if <paramref name="dateOfBirth"/> was at least <paramref name="minimumAgeYears"/> years ago.
        /// </summary>
        /// <param name="dateOfBirth">date to check.</param>
        /// <param name="minimumAgeYears">years.</param>
        /// <returns>
        /// True - if <paramref name="dateOfBirth"/> was more than <paramref name="minimumAgeYears"/> years ago, else - false.
        /// </returns>
        public static bool CheckMinimumAge(DateTime dateOfBirth, int minimumAgeYears)
        {
            var criticalDate = dateOfBirth.AddYears(minimumAgeYears);

            return DateTime.Now.CompareTo(criticalDate) > 0;
        }

        /// <summary>
        /// Finds user model with specified <paramref name="username"/> and returns it.
        /// </summary>
        /// <param name="username">unique credential of user model.</param>
        /// <returns> Existing user with <paramref name="username"/>.</returns>
        /// <exception cref="SharedException">User not found by <paramref name="username"/>.</exception>
        private async Task<User> GetByUsernameAsync(string username)
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

        /// <summary>
        /// Finds user model with specified <paramref name="userId"/> and returns it.
        /// </summary>
        /// <param name="userId">unique credential of user model.</param>
        /// <returns> Existing user with <paramref name="userId"/>.</returns>
        /// <exception cref="SharedException">User not found by <paramref name="userId"/>.</exception>
        private async Task<User> GetByIdAsync(Guid userId)
        {
            var user = await _carRentalDbContext.Users
               .Include(user => user.Roles)
               .FirstOrDefaultAsync(user => user.Id == userId);
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

        /// <summary>
        /// Checks if <paramref name="userLoginDto"/> corresponds <paramref name="user"/> and has on of client`s roles.
        /// </summary>
        /// <param name="user">existing user model.</param>
        /// <param name="userLoginDto">years.</param>
        /// <exception cref="SharedException">Client role is not approved.</exception>
        private bool ValidateClient(User user, UserLoginDto userLoginDto)
        {
            _passwordService.ValidatePassword(user.HashedPassword, user.Salt, userLoginDto.Password);

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

        /// <summary>
        /// Checks if <paramref name="userLoginDto"/> corresponds <paramref name="user"/> and has on of admin`s roles.
        /// </summary>
        /// <param name="user">existing user model.</param>
        /// <param name="userLoginDto">user prototype to be validated.</param>
        /// <exception cref="SharedException">Inconsistent user role.</exception>
        private void ValidateAdmin(User user, UserLoginDto userLoginDto)
        {
            _passwordService.ValidatePassword(user.HashedPassword, user.Salt, userLoginDto.Password);

            if (!user.Roles.Select(role => role.Role).Intersect(UserRole.AdminRoles).Any())
            {
                throw new SharedException(
                    ErrorTypes.AccessDenied,
                    "You do not have permission",
                    "User does not have admin role"
                );
            }
        }


        /// <summary>
        /// Checks if <paramref name="rolesDto"/> contains valid roles array against <paramref name="user"/> model.
        /// </summary>
        /// <param name="user">existing user model.</param>
        /// <param name="rolesDto">object. containing array of roles.</param>
        /// <exception cref="SharedException">Empty roles set.</exception>
        /// <exception cref="SharedException">Cannot specify client role without additional info.</exception>
        private void ValidateNewRoles(User user, RolesDto rolesDto)
        {
            if (rolesDto.Roles.Count < 1)
            {
                throw new SharedException(
                    ErrorTypes.Invalid,
                    "Role changing failed",
                    "User must have at least 1 role"
                );
            }

            if (rolesDto.Roles.Intersect(UserRole.ClientRoles).Any())
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

        /// <summary>
        /// Creates user model from <paramref name="userRegistrationDto"/> with None role.
        /// </summary>
        /// <param name="userRegistrationDto">user prototype.</param>
        /// <returns> Newly created user.</returns>
        private User CreateFromDto(UserRegistrationDto userRegistrationDto)
        {
            var user = userRegistrationDto.Adapt<UserRegistrationDto, User>();

            user.Salt = _passwordService.GenerateSalt();
            user.HashedPassword = _passwordService.DigestPassword(userRegistrationDto.Password, user.Salt);

            user.Roles = new List<UserRole>() { new UserRole() { Role = Roles.None } };

            return user;
        }

        /// <summary>
        /// Validates <paramref name="dateOfBirth"/> with minimum age from app config.
        /// </summary>
        /// <param name="dateOfBirth">date to check.</param>
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

        /// <summary>
        /// Checks if <paramref name="email"/> and/or <paramref name="username"/> are unique among all users.
        /// </summary>
        /// <param name="email">unique credential of user model.</param>
        /// <param name="username">unique credential of user model.</param>
        /// <exception cref="SharedException">User with such credentials already exists.</exception>
        private async Task CheckCredentialsUniqueness(string email, string username)
        {
            if (await _carRentalDbContext.Users.AnyAsync(user => user.Email.Equals(username)))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "User with such username already exists"
                );
            }

            if (await _carRentalDbContext.Users.AnyAsync(user => user.Username.Equals(email)))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "User with such email already exists"
                );
            }
        }

        /// <summary>
        /// Assigns passed array of roles to <paramref name="user"/>.
        /// </summary>
        /// <param name="user">user model to be updated.</param>
        /// <param name="rolesDto">object, containing array of roles.</param>
        private Task UpdateRolesAsync(User user, RolesDto rolesDto)
        {
            if (user.Roles.Select(role => role.Role).SequenceEqual(rolesDto.Roles))
            {
                return Task.CompletedTask;
            }

            user.Roles = rolesDto.Roles.Select(role => new UserRole() { Role = role }).ToList();

            return _carRentalDbContext.SaveChangesAsync();
        }
    }
}