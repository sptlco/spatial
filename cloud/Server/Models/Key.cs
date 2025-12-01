// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Models;

/// <summary>
/// Grants access to a <see cref="Resource"/>.
/// </summary>
[Collection("keys")]
public class Key : Resource
{
    /// <summary>
    /// The <see cref="Resource"/> access is granted to.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// A 4-digit code for verification purposes.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// The time the <see cref="Key"/> expires.
    /// </summary>
    public double Expires { get; set; } = Time.Now;
}