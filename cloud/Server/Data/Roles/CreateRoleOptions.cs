// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Roles;

/// <summary>
/// Configurable options for a new role.
/// </summary>
public class CreateRoleOptions : CreateResourceOptions
{
    /// <summary>
    /// The name of the role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The color associated with the role.
    /// </summary>
    public string Color { get; set; }

    /// <summary>
    /// A brief description of the role.
    /// </summary>
    public string? Description { get; set; }
}