// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Permissions;

/// <summary>
/// A permission with minimal properties.
/// </summary>
public class PermissionSlim
{
    /// <summary>
    /// The role the permission is assigned to.
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// The permission's scope.
    /// </summary>
    public string Scope { get; set; }
}