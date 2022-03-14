using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRentalBll.Models;
using CarRentalWeb.Configurations.JWT;
using CarRentalWeb.Configurations.JWT.Access;
using CarRentalWeb.Configurations.JWT.Refresh;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedResources.Exceptions;

namespace CarRentalWeb.Services
{
    public class JwtService
    {
        private readonly AccessJwtConfig _accessJwtConfig;
        private readonly RefreshJwtConfig _refreshJwtConfig;

        public JwtService(
            IOptions<AccessJwtConfig> accessJwtConfigOptions,
            IOptions<RefreshJwtConfig> refreshJwtConfigOptions
        )
        {
            _accessJwtConfig = accessJwtConfigOptions.Value;
            _refreshJwtConfig = refreshJwtConfigOptions.Value;
        }

        /// <summary>
        /// Gets string secret key from <paramref name="jwtParams"/> and returns its symmetric representation.
        /// </summary>
        /// <param name="jwtParams">params, containing string secret key.</param>
        /// <returns>Symmetric representation of string key.</returns>
        public static SecurityKey GetKey(GenerationParameters jwtParams)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtParams.Secret));
        }

        /// <summary>
        /// Validates token 'expiration' claim.
        /// </summary>
        /// <param name="tokenString">token to be validated.</param>
        /// <exception cref="SharedException"><paramref name="tokenString"/> is expired.</exception>
        public void ValidateTokenLifetime(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenString);
            var expires = jwtSecurityToken.ValidTo;

            var refreshJwtValidationParams = GetRefreshValidationParams();

            try
            {
                Validators.ValidateLifetime(null, expires, jwtSecurityToken, refreshJwtValidationParams);
            }
            catch (SecurityTokenExpiredException exception)
            {
                throw new SharedException(
                    ErrorTypes.AuthFailed,
                    "Invalid refresh token",
                    "Refresh token expired",
                    exception
                );
            }
        }

        /// <summary>
        /// Gets user Id from <paramref name="tokenString"/> and returns it.
        /// </summary>
        /// <param name="tokenString">token to be parsed.</param>
        /// <returns>User Id stored in <paramref name="tokenString"/>.</returns>
        /// <exception cref="SharedException">Claim 'sub' value is invalid.</exception>
        public Guid GetUserId(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokenString);
            var userIdString = GetClaimValue(jwtSecurityToken.Claims, JwtRegisteredClaimNames.Sub);

            if (!Guid.TryParse(userIdString, out var userId))
            {
                throw new SharedException(
                    ErrorTypes.Invalid,
                    "Invalid token",
                    $"Invalid 'sub' claim value"
                );
            }

            return userId;
        }

        /// <summary>
        /// Gets value of specified <paramref name="claimName"/> from <paramref name="claims"/> and returns it.
        /// </summary>
        /// <param name="claims">set of claims.</param>
        /// <param name="claimName">returned value claim.</param>
        /// <returns>Claim value, stored in <paramref name="claimName"/> of <paramref name="claims"/>.</returns>
        /// <exception cref="SharedException">Claim <paramref name="claimName"/> of <paramref name="claims"/> not found.</exception>
        public string GetClaimValue(IEnumerable<Claim> claims, string claimName)
        {
            var claimValue = claims
                .FirstOrDefault(claim => claim.Type == claimName)
                ?.Value;

            if (claimValue == null)
            {
                throw new SharedException(
                    ErrorTypes.NotFound,
                    "Invalid token",
                    $"Claim '{claimName}' not found"
                );
            }

            return claimValue;
        }

        /// <summary>
        /// Generates short-term token with minimal claims list.
        /// </summary>
        /// <param name="userId">token owner id.</param>
        /// <returns>Generated token.</returns>
        public string GenerateToken(Guid userId)
        {
            var accessJwtGenerationParams = _accessJwtConfig.GenerationParameters;

            var jwtClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: accessJwtGenerationParams.Issuer,
                audience: accessJwtGenerationParams.Audience,
                claims: jwtClaims,
                expires: DateTimeOffset.Now.UtcDateTime.AddSeconds(accessJwtGenerationParams.LifeTimeSeconds),
                signingCredentials: GetJWTCredentials(accessJwtGenerationParams)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Generates long-term token to refresh access.
        /// </summary>
        /// <param name="user">token owner.</param>
        /// <returns>Generated refresh token.</returns>
        public string GenerateRefreshToken(UserModel user)
        {
            var refreshJwtGenerationParams = _refreshJwtConfig.GenerationParameters;

            var jwtClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var refreshToken = new JwtSecurityToken(
                claims: jwtClaims,
                expires: DateTimeOffset.Now.UtcDateTime.AddSeconds(refreshJwtGenerationParams.LifeTimeSeconds),
                signingCredentials: GetJWTCredentials(refreshJwtGenerationParams)
            );

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }

        /// <summary>
        /// Generates short-term token to access resources according to <paramref name="user"/> roles.
        /// </summary>
        /// <param name="user">token owner.</param>
        /// <returns>Generated access token.</returns>
        public string GenerateAccessToken(UserModel user)
        {
            var accessJwtGenerationParams = _accessJwtConfig.GenerationParameters;

            var jwtClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            foreach (var role in user.Roles)
            {
                jwtClaims.Add(new Claim("role", role.ToString()));
            }

            var accessToken = new JwtSecurityToken(
                issuer: accessJwtGenerationParams.Issuer,
                audience: accessJwtGenerationParams.Audience,
                claims: jwtClaims,
                expires: DateTimeOffset.Now.UtcDateTime.AddSeconds(accessJwtGenerationParams.LifeTimeSeconds),
                signingCredentials: GetJWTCredentials(accessJwtGenerationParams)
            );

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }

        /// <summary>
        /// Gathers token validation params from JWT config and secret key from token generation params and returns it.
        /// </summary>
        /// <returns>Gathered token validation params.</returns>
        private TokenValidationParameters GetRefreshValidationParams()
        {
            var refreshJwtValidationParams = _refreshJwtConfig.ValidationParameters;
            refreshJwtValidationParams.IssuerSigningKey = GetKey(_refreshJwtConfig.GenerationParameters);

            return refreshJwtValidationParams;
        }

        /// <summary>
        /// Gets signing credentials for token generation from passed token generation params.
        /// </summary>
        /// <param name="jwtParams">params, which contain string secret key.</param>
        /// <returns>Signing credentials for token generation.</returns>
        private SigningCredentials GetJWTCredentials(GenerationParameters jwtParams)
        {
            return new SigningCredentials(
                GetKey(jwtParams),
                SecurityAlgorithms.HmacSha256Signature
            );
        }
    }
}