namespace SharedResources.Helpers
{
    public static class DateOperations
    {
        public static bool WasAgo(this DateTime source, int years)
        {
            var criticalDate = source.AddYears(years);
            return criticalDate < DateTime.Now;
        }
    }
}