namespace SharedResources.Configurations.JWT
{
    public abstract class GenerationParameters
    {
        public int LifeTimeSeconds { get; set; }

        public string Secret { get; set; } = null!;
    }
}