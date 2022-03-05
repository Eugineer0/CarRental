namespace SharedResources
{
    public static class ExtensionMethods
    {
        public static bool ContainsAny(this IEnumerable<Roles> roles, IEnumerable<Roles> candidate)
        {
            return roles.Intersect(candidate).Any();
        }
    }
}