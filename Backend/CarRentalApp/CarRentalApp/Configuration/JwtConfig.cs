using Microsoft.IdentityModel.Tokens;

namespace CarRentalApp.Configuration
{
    public class JwtConfig
    {
        public TokenGenerationParameters GenerationParameters { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }
    }
}