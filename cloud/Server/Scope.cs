// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud;

/// <summary>
/// Constant authorization scopes used throughout the system.
/// Scopes control access to different resources and operations.  
/// </summary>
internal static class Scope
{
    /// <summary>
    /// Scopes related to accounts.
    /// </summary>
    public class Accounts
    {
        /// <summary>
        /// Allows the user to read account data.
        /// </summary>
        public const string Read = "accounts.read";
    }

    /// <summary>
    /// Scopes related to role assignments.
    /// </summary>
    public class Assignments
    {
        /// <summary>
        /// Allows the user to create role assignments.
        /// </summary>
        public const string Create = "assignments.create";
    }

    /// <summary>
    /// Scopes related to roles.
    /// </summary>
    public class Roles
    {
        /// <summary>
        /// Allows the user to create roles.
        /// </summary>
        public const string Create = "roles.create";
    }

    /// <summary>
    /// Scopes related to permissions.
    /// </summary>
    public class Permissions
    {
        /// <summary>
        /// Allows the user to add permissions to roles.
        /// </summary>
        public const string Create = "permissions.create";
    }
}