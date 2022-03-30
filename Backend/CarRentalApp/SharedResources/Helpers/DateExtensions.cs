namespace SharedResources.Helpers
{
    public static class DateExtensions
    {
        public static bool WasYearsAgo(this DateTime start, int years)
        {
            var criticalDate = start.AddYears(years);
            return criticalDate < DateTime.Now;
        }

        public static bool CheckPeriodDuration(this DateTime start, DateTime finish, int minutes)
        {
            var criticalDate = start.AddMinutes(minutes);

            return CheckIfBefore(finish, criticalDate);
        }

        public static bool CheckIfBefore(this DateTime candidate, DateTime target)
        {
            return candidate < target;
        }

        public static TimeSpan GetPeriod(this DateTime start, DateTime finish)
        {
            return finish - start;
        }
    }
}