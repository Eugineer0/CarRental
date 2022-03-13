namespace SharedResources.Helpers
{
    public class DateOperations
    {
        public static bool CheckMinimumAge(DateTime dateOfBirth, int years)
        {
            var criticalDate = dateOfBirth.AddYears(years);

            return CheckIfBefore(DateTime.Now, criticalDate);
        }

        public static bool CheckIfBefore(DateTime target, DateTime candidate)
        {
            return target.Ticks > candidate.Ticks;
        }
    }
}