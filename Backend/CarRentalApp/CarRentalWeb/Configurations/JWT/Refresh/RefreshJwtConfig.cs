using Microsoft.IdentityModel.Tokens;

namespace CarRentalWeb.Configurations.JWT.Refresh
{
    public class RefreshJwtConfig
    {
        public const string Section = "RefreshJwtConfig";

        public RefreshTokenGenerationParameters GenerationParameters { get; set; } = null!;

        public TokenValidationParameters ValidationParameters { get; set; } = null!;
    }
}