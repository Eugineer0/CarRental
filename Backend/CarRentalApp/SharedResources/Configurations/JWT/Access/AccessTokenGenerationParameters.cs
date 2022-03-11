namespace SharedResources.Configurations.JWT.Access
{
    public class AccessTokenGenerationParameters : GenerationParameters
    {
        public string Issuer { get; set; } = null!;

        public string Audience { get; set; } = null!;
    }
}