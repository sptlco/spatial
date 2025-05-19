// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Spatial.Mathematics;
using Spatial.Simulation;
using System;

namespace Ignite.Components;

/// <summary>
/// A <see cref="IComponent"/> containing coordinate metadata for an <see cref="Entity"/>.
/// </summary>
/// <param name="X">The transform's X-component.</param>
/// <param name="Y">The transform's Y-component.</param>
/// <param name="R">The transform's rotation.</param>
public record struct Transform(
    float X = 0F,
    float Y = 0F,
    float R = 0F) : IComponent
{
    /// <summary>
    /// The transform's direction.
    /// </summary>
    public readonly byte D => GetDirection();

    /// <summary>
    /// Add two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Transform"/>.</param>
    /// <param name="right">A <see cref="Transform"/>.</param>
    /// <returns>The sum of the vectors.</returns>
    public static Transform operator +(in Transform left, in Transform right)
    {
        return new(left.X + right.X, left.Y + right.Y, left.R + right.R);
    }

    /// <summary>
    /// Subtract two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Transform"/>.</param>
    /// <param name="right">A <see cref="Transform"/>.</param>
    /// <returns>The difference of the vectors.</returns>
    public static Transform operator -(in Transform left, in Transform right)
    {
        return new(left.X - right.X, left.Y - right.Y, left.R - right.R);
    }

    /// <summary>
    /// Multiply two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Transform"/>.</param>
    /// <param name="right">A <see cref="Transform"/>.</param>
    /// <returns>The product of the vectors.</returns>
    public static Transform operator *(in Transform left, in Transform right)
    {
        return new(left.X * right.X, left.Y * right.Y, left.R * right.R);
    }

    /// <summary>
    /// Divide two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Transform"/>.</param>
    /// <param name="right">A <see cref="Transform"/>.</param>
    /// <returns>The quotient of the vectors.</returns>
    public static Transform operator /(in Transform left, in Transform right)
    {
        return new(left.X / right.X, left.Y / right.Y, left.R / right.R);
    }

    /// <summary>
    /// Add two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Transform"/>.</param>
    /// <param name="right">A <see cref="Velocity"/>.</param>
    /// <returns>The sum of the vectors.</returns>
    public static Transform operator +(in Transform left, in Velocity right)
    {
        return new(left.X + right.X, left.Y + right.Y, left.R + right.R);
    }

    /// <summary>
    /// Subtract two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Transform"/>.</param>
    /// <param name="right">A <see cref="Velocity"/>.</param>
    /// <returns>The difference of the vectors.</returns>
    public static Transform operator -(in Transform left, in Velocity right)
    {
        return new(left.X - right.X, left.Y - right.Y, left.R - right.R);
    }

    /// <summary>
    /// Multiply two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Transform"/>.</param>
    /// <param name="right">A <see cref="Velocity"/>.</param>
    /// <returns>The product of the vectors.</returns>
    public static Transform operator *(in Transform left, in Velocity right)
    {
        return new(left.X * right.X, left.Y * right.Y, left.R * right.R);
    }

    /// <summary>
    /// Divide two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Transform"/>.</param>
    /// <param name="right">A <see cref="Velocity"/>.</param>
    /// <returns>The quotient of the vectors.</returns>
    public static Transform operator /(in Transform left, in Velocity right)
    {
        return new(left.X / right.X, left.Y / right.Y, left.R / right.R);
    }

    /// <summary>
    /// Add a scalar to a <see cref="Transform"/>.
    /// </summary>
    /// <param name="vector">A <see cref="Transform"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The sum of the <see cref="Transform"/> and the scalar.</returns>
    public static Transform operator +(in Transform vector, in float scalar)
    {
        return new(vector.X + scalar, vector.Y + scalar, vector.R + scalar);
    }

    /// <summary>
    /// Subtract a scalar from a <see cref="Transform"/>.
    /// </summary>
    /// <param name="vector">A <see cref="Transform"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The difference of the <see cref="Transform"/> and the scalar.</returns>
    public static Transform operator -(in Transform vector, in float scalar)
    {
        return new(vector.X - scalar, vector.Y - scalar, vector.R - scalar);
    }

    /// <summary>
    /// Multiply a <see cref="Transform"/> by a scalar.
    /// </summary>
    /// <param name="vector">A <see cref="Transform"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The product of the <see cref="Transform"/> and the scalar.</returns>
    public static Transform operator *(in Transform vector, in float scalar)
    {
        return new(vector.X * scalar, vector.Y * scalar, vector.R * scalar);
    }

    /// <summary>
    /// Divide a <see cref="Transform"/> by a scalar.
    /// </summary>
    /// <param name="vector">A <see cref="Transform"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The quotient of the <see cref="Transform"/> and the scalar.</returns>
    public static Transform operator /(in Transform vector, in float scalar)
    {
        return new(vector.X / scalar, vector.Y / scalar, vector.R / scalar);
    }

    /// <summary>
    /// Create a new <see cref="Transform"/>.
    /// </summary>
    /// <param name="point">A <see cref="Point2D"/>.</param>
    public static implicit operator Transform(Point2D point) => new Transform(point.X, point.Y);

    /// <summary>
    /// Create a new <see cref="Point2D"/>.
    /// </summary>
    /// <param name="transform">A <see cref="Transform"/>.</param>
    public static implicit operator Point2D(Transform transform) => new Point2D(transform.X, transform.Y);

    /// <summary>
    /// Calculate the direction from point <paramref name="a"/> to destination <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The starting position.</param>
    /// <param name="b">The target position.</param>
    /// <returns>The direction from point <paramref name="a"/> to destination <paramref name="b"/>.</returns>
    public static float Heading(Transform a, Destination b)
    {
        return (270 - Point2D.Heading(a.X, a.Y, b.X, b.Y) + 360) % 360;
    }

    /// <summary>
    /// Get a random <see cref="Transform"/> within a <see cref="Region"/>.
    /// </summary>
    /// <param name="region">A <see cref="Region"/>.</param>
    /// <returns>A random <see cref="Transform"/>.</returns>
    public static Transform Within(in Region region)
    {
        static float random() => Strong.Float(0.0F, 1.0F);

        var rotation = Strong.Float(0.0F, 360.0F);

        switch (region.Shape)
        {
            case Shape.Rectangle:
                var x = (random() * region.Width) - (region.Width / 2);
                var y = (random() * region.Height) - (region.Height / 2);

                return new Transform(
                    X: region.X + (x * region.Cosine - y * region.Sine),
                    Y: region.Y + (x * region.Sine + y * region.Cosine),
                    R: rotation);

            case Shape.Circle:
                var angle = random() * 2 * Math.PI;
                var radius = (float) Math.Sqrt(random()) * region.Radius;

                return new Transform(
                    X: region.X + radius * (float) Math.Cos(angle),
                    Y: region.Y + radius * (float) Math.Sin(angle),
                    R: rotation);

            default:
                throw new InvalidOperationException("The region type is unsupported.");
        }
    }

    /// <summary>
    /// Get a random <see cref="Transform"/> within an <see cref="Area"/>.
    /// </summary>
    /// <param name="area">An <see cref="Area"/>.</param>
    /// <returns>A random <see cref="Transform"/>.</returns>
    public static Transform Within(in Area area)
    {
        static float random() => Strong.Float(0.0F, 1.0F);

        var rotation = Strong.Float(0.0F, 360.0F);

        switch (area)
        {
            case Rectangle rectangle:
                var x = (random() * rectangle.Size.X) - (rectangle.Size.X / 2);
                var y = (random() * rectangle.Size.Y) - (rectangle.Size.Y / 2);

                return new Transform(
                    X: rectangle.Position.X + (x * rectangle.Cosine - y * rectangle.Sine),
                    Y: rectangle.Position.Y + (x * rectangle.Sine + y * rectangle.Cosine),
                    R: rotation);

            case Circle circle:
                var angle = random() * 2 * Math.PI;
                var radius = (float) Math.Sqrt(random()) * circle.Radius;

                return new Transform(
                    X: circle.Position.X + radius * (float) Math.Cos(angle),
                    Y: circle.Position.Y + radius * (float) Math.Sin(angle),
                    R: rotation);

            default:
                throw new InvalidOperationException("The area type is unsupported.");
        }
    }

    /// <summary>
    /// Get a random <see cref="Transform"/> around a <see cref="Transform"/>.
    /// </summary>
    /// <param name="position">A <see cref="Transform"/>.</param>
    /// <param name="range">Distance from <paramref name="position"/>.</param>
    /// <returns>A random <see cref="Transform"/>.</returns>
    public static Transform Around(in Transform position, in float range)
    {
        var left = position.X - range;
        var right = position.X + range;
        var top = position.Y + range;
        var bottom = position.Y - range;

        return new Transform(X: Strong.Float(left, right), Y: Strong.Float(bottom, top), R: Strong.Float(0.0F, 360.0F));
    }

    /// <summary>
    /// Convert the <see cref="Transform"/> to a <see cref="string"/>.
    /// </summary>
    /// <returns>A <see cref="string"/>.</returns>
    public override readonly string ToString()
    {
        return $"({X}, {Y})";
    }

    private readonly byte GetDirection()
    {
        var direction = (int) (R / 2);

        if (direction < 0)
        {
            direction |= (direction - 76) & 0xFF;
        }

        return (byte) direction;
    }
}