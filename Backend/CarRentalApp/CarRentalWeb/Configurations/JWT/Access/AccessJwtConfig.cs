using Microsoft.IdentityModel.Tokens;

namespace CarRentalWeb.Configurations.JWT.Access
{
    public class AccessJwtConfig
    {
        public const string Section = "AccessJwtConfig";

        public AccessTokenGenerationParameters GenerationParameters { get; set; } = null!;

        public TokenValidationParameters ValidationParameters { get; set; } = null!;
    }
}