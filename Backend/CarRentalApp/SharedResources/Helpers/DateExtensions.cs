namespace SharedResources.Helpers
{
    public static class DateExtensions
    {
        public static bool WasYearsAgo(this DateTime start, int years)
        {
            var criticalDate = start.AddYears(years);
            return criticalDate < DateTime.Now;
        }

        public static bool HasDurationUntil(this DateTime start, DateTime finish, int minutes)
        {
            var criticalDate = start.AddMinutes(minutes);
            return finish <= criticalDate;
        }
    }
}