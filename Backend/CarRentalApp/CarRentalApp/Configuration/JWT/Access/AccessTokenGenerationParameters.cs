namespace CarRentalApp.Configuration.JWT.Access
{
    public class AccessTokenGenerationParameters : GenerationParameters
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}