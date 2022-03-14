namespace SharedResources.Helpers
{
    public static class DateExtensions
    {
        public static bool HasDurationYears(this DateTime start, DateTime finish, int years)
        {
            var criticalDate = start.AddYears(years);
            return criticalDate < finish;
        }
    }
}