// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Identity.Authorization;

/// <summary>
/// ...
/// </summary>
[Collection("assignments")]
public class Assignment : Resource
{
    /// <summary>
    /// The user the <see cref="Role"/> is assigned to.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// A <see cref="Role"/> identifier.
    /// </summary>
    public string Role { get; set; }
}