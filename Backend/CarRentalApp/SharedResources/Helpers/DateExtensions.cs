namespace SharedResources.Helpers
{
    public static class DateExtensions
    {
        public static bool WasYearsAgo(this DateTime start, int years)
        {
            var criticalDate = start.AddYears(years);
            return criticalDate < DateTime.Now;
        }
    }
}