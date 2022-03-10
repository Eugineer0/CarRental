using Microsoft.IdentityModel.Tokens;

namespace SharedResources.Configurations.JWT.Refresh
{
    public class RefreshJwtConfig
    {
        public const string Section = "RefreshJwtConfig";

        public RefreshTokenGenerationParameters GenerationParameters { get; set; } = null!;

        public TokenValidationParameters ValidationParameters { get; set; } = null!;
    }
}