using SharedResources.EnumsAndConstants;

namespace SharedResources.Helpers
{
    public static class EnumerableExtensions
    {
        public static bool ContainsAny(this IEnumerable<Role> candidate, IEnumerable<Role> target)
        {
            return target.FirstOrDefault(candidate.Contains) != default;
        }

        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> target)
        {
            return target.All(source.Contains);
        }
    }
}