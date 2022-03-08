namespace SharedResources
{
    public static class ExtensionMethods
    {
        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> target)
        {
            return source.Intersect(target).Any();
        }

        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> target)
        {
            return target.All(source.Contains);
        }
    }
}