namespace SharedResources.Helpers
{
    public static class SetOperations
    {
        public static bool ContainsAny<T>(this IEnumerable<T> candidate, IEnumerable<T> target)
        {
            return target.FirstOrDefault(candidate.Contains) == null;
        }
    }
}