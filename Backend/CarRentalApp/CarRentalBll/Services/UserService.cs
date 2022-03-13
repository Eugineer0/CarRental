using CarRentalBll.Configurations;
using CarRentalBll.Exceptions;
using CarRentalBll.Models;
using CarRentalDal.Contexts;
using CarRentalDal.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedResources.Configurations;
using SharedResources.EnumsAndConstants;
using SharedResources.Helpers;

namespace CarRentalBll.Services
{
    public class UserService
    {
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;
        private readonly UserRequirements _userRequirements;
        private readonly CarRentalDbContext _carRentalDbContext;

        public UserService(
            PasswordService passwordService,
            IOptions<UserRequirements> userRequirements,
            CarRentalDbContext carRentalDbContext,
            TokenService tokenService
        )
        {
            _passwordService = passwordService;
            _carRentalDbContext = carRentalDbContext;
            _tokenService = tokenService;
            _userRequirements = userRequirements.Value;
        }

        /// <summary>
        /// Adds <paramref name="driverLicenseSerialNumber"/> to existing user with specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">unique credential of user to be updated</param>
        /// <param name="driverLicenseSerialNumber">value to fill in user field</param>
        public async Task AddDriverLicenseByAsync(Guid userId, string driverLicenseSerialNumber)
        {
            var user = await GetByIdAsync(userId);

            user.DriverLicenseSerialNumber = driverLicenseSerialNumber;

            await _carRentalDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Validates passed user prototype as client and returns existing user model.
        /// </summary>
        /// <param name="loginModel">user prototype to login.</param>
        /// <returns>Existing user model.</returns>
        /// <exception cref="SharedException">User must specify driver license field</exception>
        public async Task<UserModel> GetValidClientAsync(LoginModel loginModel)
        {
            var user = await GetByUsernameAsync(loginModel.Username);
            _passwordService.ValidatePassword(user.HashedPassword, user.Salt, loginModel.Password);

            var userModel = user.Adapt<User, UserModel>();
            if (ValidateAsClient(userModel))
            {
                return userModel;
            }

            var token = _tokenService.GenerateRefreshToken(userModel);
            throw new SharedException(
                ErrorTypes.AdditionalDataRequired,
                token,
                "Registration completion required"
            );
        }

        /// <summary>
        /// Validates passed user prototype as admin and returns existing user model.
        /// </summary>
        /// <param name="loginModel">user prototype to validate.</param>
        /// <returns>Existing user model.</returns>
        public async Task<UserModel> GetValidAdminAsync(LoginModel loginModel)
        {
            var user = await GetByUsernameAsync(loginModel.Username);
            _passwordService.ValidatePassword(user.HashedPassword, user.Salt, loginModel.Password);

            var userModel = user.Adapt<User, UserModel>();
            if (userModel.Roles.ContainsAny(RolesConstants.AdminRoles))
            {
                return userModel;
            }

            throw new SharedException(
                ErrorTypes.AccessDenied,
                "You do not have permission",
                "User does not have admin role"
            );
        }

        /// <summary>
        /// Checks uniqueness of credentials, saves created user and returns its model.
        /// </summary>
        /// <param name="registrationModel">user prototype to register.</param>
        /// <returns>Model of saved user.</returns>
        public async Task<UserModel> RegisterAsync(RegistrationModel registrationModel)
        {
            await CheckCredentialsUniqueness(registrationModel.Email, registrationModel.Username);
            var user = CreateEntity(registrationModel);
            _carRentalDbContext.Users.Add(user);
            await _carRentalDbContext.SaveChangesAsync();

            return user.Adapt<UserModel>();
        }

        /// <summary>
        /// Finds user with specified <paramref name="userId"/> and returns it model representation.
        /// </summary>
        /// <param name="userId">unique credential of user.</param>
        /// <returns>Model of existing user.</returns>
        public async Task<UserModel> GetByAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            return user.Adapt<UserModel>();
        }

        /// <summary>
        /// Finds user with specified <paramref name="username"/> and returns it model representation.
        /// </summary>
        /// <param name="username">unique credential of user.</param>
        /// <returns>Model of existing user.</returns>
        public async Task<UserModel> GetByAsync(string username)
        {
            var user = await GetByUsernameAsync(username);
            return user.Adapt<UserModel>();
        }

        /// <summary>
        /// Updates user found by passed username with client role.
        /// </summary>
        /// <param name="username">unique credential of user.</param>
        /// <exception cref="SharedException">User with such <paramref name="username"/> does not meet requirements.</exception>
        public async Task ApproveClientAsync(string username)
        {
            var user = await GetByUsernameAsync(username);

            if (user.Roles
                .Select(userRole => userRole.Role)
                .ContainsAny(RolesConstants.ClientRoles))
            {
                return;
            }

            ValidateClientAge(user.DateOfBirth);

            if (user.DriverLicenseSerialNumber == null)
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "Client approval failed",
                    "User cannot become a client without specifying DriverLicenseSerialNumber"
                );
            }

            user.Roles.Add(new UserRole() { Role = Roles.Client });
            user.Roles.RemoveAll(role => role.Role == Roles.None);
            await _carRentalDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Validates passed <paramref name="roles"/> and assign them to existing user found by <paramref name="username"/>.
        /// </summary>
        /// <param name="username">unique credential of user.</param>
        /// <param name="roles">object, containing array of roles.</param>
        /// <exception cref="SharedException">Cannot specify client role without additional info.</exception>
        public async Task ModifyRolesAsync(string username, IReadOnlySet<Roles> roles)
        {
            ValidateRoles(roles);
            var user = await GetByUsernameAsync(username);

            if (roles.ContainsAny(RolesConstants.ClientRoles))
            {
                ValidateClientAge(user.DateOfBirth);

                if (user.DriverLicenseSerialNumber == null)
                {
                    throw new SharedException(
                        ErrorTypes.Conflict,
                        "Roles modifying failed",
                        "User cannot become a client without specifying DriverLicenseSerialNumber"
                    );
                }
            }

            if (roles.ContainsAny(RolesConstants.AdminRoles))
            {
                ValidateAdminAge(user.DateOfBirth);
            }

            await UpdateRolesAsync(user, roles);
        }

        /// <summary>
        /// Finds user with specified <paramref name="username"/> and returns it.
        /// </summary>
        /// <param name="username">unique credential of user.</param>
        /// <returns> Existing user with <paramref name="username"/>.</returns>
        /// <exception cref="SharedException">User not found by <paramref name="username"/>.</exception>
        private async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _carRentalDbContext.Users
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Username == username);
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
        /// Finds user with specified <paramref name="userId"/> and returns it.
        /// </summary>
        /// <param name="userId">unique credential of user.</param>
        /// <returns>Existing user with <paramref name="userId"/>.</returns>
        /// <exception cref="SharedException">User not found by <paramref name="userId"/>.</exception>
        private async Task<User> GetByIdAsync(Guid userId)
        {
            var user = await _carRentalDbContext.Users
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Id == userId);
            if (user == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "User not found",
                    "User with such Id not found"
                );
            }

            return user;
        }

        /// <summary>
        /// Checks if <paramref name="userModel"/> can be logged in as client.
        /// </summary>
        /// <param name="userModel">existing entity model.</param>
        /// <returns>True - if user is already a client, False - if user cannot be a client.</returns>
        /// <exception cref="SharedException">Client role is not approved.</exception>
        private bool ValidateAsClient(UserModel userModel)
        {
            if (userModel.Roles.ContainsAny(RolesConstants.ClientRoles))
            {
                return true;
            }

            if (userModel.DriverLicenseSerialNumber == null)
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
        /// Checks if <paramref name="roles"/> are valid and can be assigned to user.
        /// </summary>
        /// <param name="roles">set of roles to validate.</param>
        /// <exception cref="SharedException">Empty roles set.</exception>
        /// <exception cref="SharedException">Inconsistent role set.</exception>
        private void ValidateRoles(IReadOnlySet<Roles> roles)
        {
            if (roles.Count < 1)
            {
                throw new SharedException(
                    ErrorTypes.Invalid,
                    "Roles modifying failed",
                    "User must have at least 1 role"
                );
            }

            if (roles.Contains(Roles.None) && roles.Count > 1)
            {
                throw new SharedException(
                    ErrorTypes.Invalid,
                    "Roles modifying failed",
                    "User must have either single 'None' role or other roles"
                );
            }
        }

        /// <summary>
        /// Creates user from <paramref name="registrationModel"/> with None role.
        /// </summary>
        /// <param name="registrationModel">user prototype.</param>
        /// <returns>Newly created user.</returns>
        private User CreateEntity(RegistrationModel registrationModel)
        {
            var user = registrationModel.Adapt<RegistrationModel, User>();

            user.Salt = _passwordService.GenerateSalt();
            user.HashedPassword = _passwordService.DigestPassword(registrationModel.Password, user.Salt);

            user.Roles = new List<UserRole>() { new() { Role = Roles.None } };

            return user;
        }

        /// <summary>
        /// Validates client`s <paramref name="dateOfBirth"/> with required minimum age from app config.
        /// </summary>
        /// <param name="dateOfBirth">date to check.</param>
        /// <exception cref="SharedException">Invalid user age.</exception>
        private void ValidateClientAge(DateTime dateOfBirth)
        {
            var minimumAge = _userRequirements.ClientMinimumAge;

            if (DateOperations.CheckMinimumAge(dateOfBirth, minimumAge))
            {
                return;
            }

            throw new SharedException(
                ErrorTypes.Conflict,
                "User`s data does not meet requirements",
                $"Client`s age must be greater than {minimumAge}"
            );
        }

        /// <summary>
        /// Validates admin`s <paramref name="dateOfBirth"/> with required minimum age from app config.
        /// </summary>
        /// <param name="dateOfBirth">date to check.</param>
        /// <exception cref="SharedException">Invalid user age.</exception>
        private void ValidateAdminAge(DateTime dateOfBirth)
        {
            var minimumAge = _userRequirements.AdminMinimumAge;

            if (DateOperations.CheckMinimumAge(dateOfBirth, minimumAge))
            {
                return;
            }

            throw new SharedException(
                ErrorTypes.Conflict,
                "User`s data does not meet requirements",
                $"Admin`s age must be greater than {minimumAge}"
            );
        }

        /// <summary>
        /// Checks if <paramref name="email"/> and <paramref name="username"/> are separately unique among all users.
        /// </summary>
        /// <param name="email">unique credential of user.</param>
        /// <param name="username">unique credential of user.</param>
        /// <exception cref="SharedException">User with such credentials already exists.</exception>
        private async Task CheckCredentialsUniqueness(string email, string username)
        {
            if (await _carRentalDbContext.Users.AnyAsync(user => user.Username == username))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "User with such username already exists"
                );
            }

            if (await _carRentalDbContext.Users.AnyAsync(user => user.Email == email))
            {
                throw new SharedException(
                    ErrorTypes.Conflict,
                    "User with such email already exists"
                );
            }
        }

        /// <summary>
        /// Assigns passed set of roles to <paramref name="user"/>.
        /// </summary>
        /// <param name="user">entity to be updated.</param>
        /// <param name="roles">set of roles.</param>
        private Task UpdateRolesAsync(User user, IReadOnlySet<Roles> roles)
        {
            if (user.Roles.Select(role => role.Role).SequenceEqual(roles))
            {
                return Task.CompletedTask;
            }

            user.Roles = roles.Select(role => new UserRole() { Role = role }).ToList();

            return _carRentalDbContext.SaveChangesAsync();
        }
    }
}