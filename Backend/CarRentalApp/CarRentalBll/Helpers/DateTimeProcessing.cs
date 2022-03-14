namespace CarRentalBll.Helpers
{
    public static class DateTimeProcessing
    {
        public static bool CheckMinimumAge(DateTime dateOfBirth, int years)
        {
            var criticalDate = dateOfBirth.AddYears(years);

            return CheckIfBefore(DateTime.Now, criticalDate);
        }

        public static bool CheckPeriodDuration(DateTime start, DateTime finish, int minutes)
        {
            var criticalDate = start.AddMinutes(minutes);

            return CheckIfBefore(finish, criticalDate);
        }

        public static bool CheckIfBefore(DateTime target, DateTime candidate)
        {
            return candidate < target;
        }

        public static bool CheckIfAfter(DateTime target, DateTime candidate)
        {
            return target < candidate;
        }

        public static TimeSpan GetPeriod(DateTime startRent, DateTime finishRent)
        {
            return finishRent - startRent;
        }
    }
}