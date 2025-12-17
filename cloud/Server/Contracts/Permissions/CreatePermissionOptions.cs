// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Contracts.Permissions;

/// <summary>
/// Configurable options for a permission.
/// </summary>
public class CreatePermissionOptions
{
    /// <summary>
    /// The role the permission is granted to.
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// The scope to grant access to.
    /// </summary>
    public string Scope { get; set; }
}