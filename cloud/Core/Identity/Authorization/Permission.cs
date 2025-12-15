// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Identity.Authorization;

/// <summary>
/// ...
/// </summary>
[Collection("permissions")]
public class Permission : Record
{
    /// <summary>
    /// The <see cref="Role"/> granted the <see cref="Permission"/>.
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// The permission's value (e.g. users.create).
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// A message describing the <see cref="Permission"/>.
    /// </summary>
    public string Description { get; set; }
}