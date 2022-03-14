﻿using SharedResources.EnumsAndConstants;

namespace SharedResources.Helpers
{
    public static class SetOperations
    {
        public static bool ContainsAny(this IEnumerable<Roles> candidate, IEnumerable<Roles> target)
        {
            return target.FirstOrDefault(candidate.Contains) == default;
        }
    }
}