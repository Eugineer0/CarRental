namespace CarRentalApp.Configuration
{
    public class JwtConfig
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int TokenLifeTimeSeconds { get; set; }

        public string Secret { get; set; }
    }
}
