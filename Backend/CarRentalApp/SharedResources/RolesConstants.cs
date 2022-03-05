﻿namespace SharedResources
{
    public static class RolesConstants
    {
        public const string AdminRolesString = "Admin, SuperAdmin";
        public const string ClientRolesString = "Client";

        public static readonly IEnumerable<Roles> AdminRoles = new[] { Roles.Admin, Roles.SuperAdmin };
        public static readonly IEnumerable<Roles> ClientRoles = new[] { Roles.Client };
    }
}