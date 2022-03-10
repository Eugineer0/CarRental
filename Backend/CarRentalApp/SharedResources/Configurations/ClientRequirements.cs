namespace CarRentalBll.Configuration
{
    public class ClientRequirements
    {
        public const string Section = "ClientRequirements";

        public int  MinimumRentalPeriodDurationMinutes { get; set; }

        public int MinimumAge { get; set; }
    }
}