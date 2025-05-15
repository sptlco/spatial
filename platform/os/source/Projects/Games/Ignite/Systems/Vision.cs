// Copyright © Spatial. All rights reserved.

using Ignite.Models;

namespace Ignite.Systems;

/// <summary>
/// A <see cref="System"/> that governs <see cref="Object"/> visibility.
/// </summary>
public class Vision : System
{
    /// <summary>
    /// Create a new <see cref="Vision"/> <see cref="System"/>.
    /// </summary>
    /// <param name="map">The enclosing <see cref="Map"/>.</param>
    public Vision(Map map) : base(map)
    {
    }
}