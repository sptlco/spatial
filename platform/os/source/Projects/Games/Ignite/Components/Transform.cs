// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// A <see cref="IComponent"/> containing spatial metadata for an <see cref="Entity"/>.
/// </summary>
/// <param name="X">The entity's X-coordinate.</param>
/// <param name="Y">The entity's Y-coordinate.</param>
/// <param name="Rotation">The entity's degree of rotation.</param>
/// <param name="Size">The size of the <see cref="Entity"/>.</param>
public record struct Transform(
    float X = 0F,
    float Y = 0F,
    float Rotation = 0F,
    float Size = 0F) : IComponent
{
    /// <summary>
    /// The entity's direction.
    /// </summary>
    public readonly byte Direction => GetDirection();

    private readonly byte GetDirection()
    {
        var direction = (int) (Rotation / 2);

        if (direction < 0)
        {
            direction |= (direction - 76) & 0xFF;
        }

        return (byte) direction;
    }
}