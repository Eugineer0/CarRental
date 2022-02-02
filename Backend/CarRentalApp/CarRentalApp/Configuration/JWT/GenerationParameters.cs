namespace CarRentalApp.Configuration.JWT
{
    public abstract class GenerationParameters
    {
        public int LifeTimeSeconds { get; set; }

        public string Secret { get; set; }
    }
}
