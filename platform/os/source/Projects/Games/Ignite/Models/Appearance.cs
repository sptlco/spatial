// Copyright © Spatial. All rights reserved.

namespace Ignite.Models;

/// <summary>
/// The physical appearance of a <see cref="Character"/>.
/// </summary>
public class Appearance
{
    /// <summary>
    /// The avatar's hair.
    /// </summary>
    public (byte Style, byte Color) Hair { get; set; }

    /// <summary>
    /// The avatar's face.
    /// </summary>
    public byte Face { get; set; }
}