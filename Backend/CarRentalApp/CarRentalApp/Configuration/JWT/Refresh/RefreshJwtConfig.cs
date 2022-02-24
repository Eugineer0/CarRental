using Microsoft.IdentityModel.Tokens;

namespace CarRentalApp.Configuration.JWT.Refresh
{
    public class RefreshJwtConfig
    {
        public const string Section = "RefreshJwtConfig";

        public RefreshTokenGenerationParameters GenerationParameters { get; set; } = null!;

        public TokenValidationParameters ValidationParameters { get; set; } = null!;
    }
}