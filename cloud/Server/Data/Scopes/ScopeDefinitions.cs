// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Scopes;

/// <summary>
/// Constant authorization scopes used throughout the system.
/// Scopes control access to different resources and operations.  
/// </summary>
public partial class Scope
{
    /// <summary>
    /// Scopes related to accounts.
    /// </summary>
    public class Accounts
    {
        /// <summary>
        /// Create an account.
        /// </summary>
        [Metadata("person_add", "Create an account.")]
        public const string Create = "accounts.create";

        /// <summary>
        /// Update an account.
        /// </summary>
        [Metadata("person_edit", "Update an account.")]
        public const string Update = "accounts.update";

        /// <summary>
        /// Delete an account.
        /// </summary>
        [Metadata("person_remove", "Delete an account.")]
        public const string Delete = "accounts.delete";
    }

    /// <summary>
    /// Scopes related to assignments.
    /// </summary>
    public class Assignments
    {
        /// <summary>
        /// Get a list of assignments.
        /// </summary>
        [Metadata("Get a list of assignments.")]
        public const string List = "assignments.list";

        /// <summary>
        /// Patch role assignments.
        /// </summary>
        [Metadata("Patch role assignments.")]
        public const string Patch = "assignments.patch";
    }

    /// <summary>
    /// Scopes related to metric data.
    /// </summary>
    public class Metrics
    {
        /// <summary>
        /// Read metric data points.
        /// </summary>
        [Metadata("Read metric data points.")]
        public const string Read = "metrics.read";

        /// <summary>
        /// Write metric data points.
        /// </summary>
        [Metadata("Write metric data points.")]
        public const string Write = "metrics.write";
    }

    /// <summary>
    /// Scopes related to permissions.
    /// </summary>
    public class Permissions
    {
        /// <summary>
        /// Update permissions.
        /// </summary>
        [Metadata("Update permissions.")]
        public const string Update = "permissions.update";

        /// <summary>
        /// Get a list of permissions.
        /// </summary>
        [Metadata("Get a list of permissions.")]
        public const string List = "permissions.list";
    }

    /// <summary>
    /// Scopes related to roles.
    /// </summary>
    public class Roles
    {
        /// <summary>
        /// Create a role.
        /// </summary>
        [Metadata("group_add", "Create a role.")]
        public const string Create = "roles.create";

        /// <summary>
        /// Update a role.
        /// </summary>
        [Metadata("person_edit", "Update a role.")]
        public const string Update = "roles.update";

        /// <summary>
        /// Get a list of roles.
        /// </summary>
        [Metadata("Get a list of roles.")]
        public const string List = "roles.list";

        /// <summary>
        /// Delete a role.
        /// </summary>
        [Metadata("person_remove", "Delete a role.")]
        public const string Delete = "roles.delete";
    }

    public class Scopes
    {
        /// <summary>
        /// Get a list of scopes.
        /// </summary>
        [Metadata("Get a list of scopes.")]
        public const string List = "scopes.list";
    }

    /// <summary>
    /// Scopes related to users.
    /// </summary>
    public class Users
    {
        /// <summary>
        /// Get a list of users.
        /// </summary>
        [Metadata("Get a list of users.")]
        public const string List = "users.list";
    }
}