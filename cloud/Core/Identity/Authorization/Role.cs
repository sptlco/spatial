// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Identity.Authorization;

/// <summary>
/// ...
/// </summary>
[Collection("roles")]
public class Role : Resource
{
    /// <summary>
    /// The role's display name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// What users with this role do.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The color associated with the role.
    /// </summary>
    public string? Color { get; set; }
}