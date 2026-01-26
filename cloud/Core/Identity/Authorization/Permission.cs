// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Identity.Authorization;

/// <summary>
/// Scoped access granted to a <see cref="Role"/>.
/// </summary>
[Collection("permissions")]
public class Permission : Resource
{
    /// <summary>
    /// The <see cref="Role"/> granted the <see cref="Permission"/>.
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// The scope accessible by the <see cref="Role"/> (e.g. users.create).
    /// </summary>
    public string Scope { get; set; }
}