// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// A movement vector for an <see cref="Entity"/>.
/// </summary>
/// <param name="X">The vector's X-component.</param>
/// <param name="Y">The vector's Y-component.</param>
/// <param name="R">The vector's rotation.</param>
public record struct Velocity(float X = 0.0F, float Y = 0.0F, float R = 0.0F) : IComponent
{
    /// <summary>
    /// Add a scalar to a <see cref="Velocity"/>.
    /// </summary>
    /// <param name="vector">A <see cref="Velocity"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The sum of the <see cref="Velocity"/> and the scalar.</returns>
    public static Velocity operator +(in Velocity vector, in float scalar)
    {
        return new(vector.X + scalar, vector.Y + scalar, vector.R + scalar);
    }

    /// <summary>
    /// Subtract a scalar from a <see cref="Velocity"/>.
    /// </summary>
    /// <param name="vector">A <see cref="Velocity"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The difference of the <see cref="Velocity"/> and the scalar.</returns>
    public static Velocity operator -(in Velocity vector, in float scalar)
    {
        return new(vector.X - scalar, vector.Y - scalar, vector.R - scalar);
    }

    /// <summary>
    /// Multiply a <see cref="Velocity"/> by a scalar.
    /// </summary>
    /// <param name="vector">A <see cref="Velocity"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The product of the <see cref="Velocity"/> and the scalar.</returns>
    public static Velocity operator *(in Velocity vector, in float scalar)
    {
        return new(vector.X * scalar, vector.Y * scalar, vector.R * scalar);
    }

    /// <summary>
    /// Divide a <see cref="Velocity"/> by a scalar.
    /// </summary>
    /// <param name="vector">A <see cref="Velocity"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The quotient of the <see cref="Velocity"/> and the scalar.</returns>
    public static Velocity operator /(in Velocity vector, in float scalar)
    {
        return new(vector.X / scalar, vector.Y / scalar, vector.R / scalar);
    }
}