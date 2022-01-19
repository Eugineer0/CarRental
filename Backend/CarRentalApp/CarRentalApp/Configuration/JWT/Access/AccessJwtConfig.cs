using Microsoft.IdentityModel.Tokens;

namespace CarRentalApp.Configuration.JWT.Access
{
    public class AccessJwtConfig
    {
        public const string Section = "AccessJwtConfig";
        public AccessTokenGenerationParameters GenerationParameters { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }
    }
}
