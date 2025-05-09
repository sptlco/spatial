// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// The final position of a moving <see cref="Entity"/>.
/// </summary>
/// <param name="X">The entity's X-coordinate.</param>
/// <param name="Y">The entity's Y-coordinate.</param>
public record struct Destination(float X, float Y) : IComponent
{
    /// <summary>
    /// Convert the <see cref="Destination"/> to a <see cref="string"/>.
    /// </summary>
    /// <returns>A <see cref="string"/>.</returns>
    public override readonly string ToString()
    {
        return $"({X}, {Y})";
    }
}