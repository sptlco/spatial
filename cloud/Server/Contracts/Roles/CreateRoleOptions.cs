// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Contracts.Roles;

/// <summary>
/// Configurable options for a new role.
/// </summary>
public class CreateRoleOptions
{
    /// <summary>
    /// The name of the role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A brief description of the role.
    /// </summary>
    public string Description { get; set; }
}