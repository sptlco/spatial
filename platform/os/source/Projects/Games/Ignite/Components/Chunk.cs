// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// An area of a <see cref="Map"/> that can be dynamically loaded and unloaded.
/// </summary>
/// <param name="X">The chunk's X-coordinate.</param>
/// <param name="Y">The chunk's Y-coordinate.</param>
public record struct Chunk(
    int X,
    int Y) : IComponent
{
    /// <summary>
    /// Convert the <see cref="Chunk"/> to a string.
    /// </summary>
    /// <returns>A string representation of the <see cref="Chunk"/>.</returns>
    public override readonly string ToString()
    {
        return $"{(char) ('A' + Y)}{X}";
    }
}