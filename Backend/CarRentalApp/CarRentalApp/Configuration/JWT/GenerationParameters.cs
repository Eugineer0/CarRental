namespace CarRentalApp.Configuration.JWT
{
    public abstract class GenerationParameters
    {
        public virtual int LifeTimeSeconds { get; set; }

        public virtual string Secret { get; set; }
    }
}
