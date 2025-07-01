// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Mathematics;

/// <summary>
/// A two-dimensional vector.
/// </summary>
/// <param name="X">The vector's X-component.</param>
/// <param name="Y">The vector's Y-component.</param>
public record struct Point2D(float X, float Y)
{
    /// <summary>
    /// Add two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Point2D"/>.</param>
    /// <param name="right">A <see cref="Point2D"/>.</param>
    /// <returns>The sum of the vectors.</returns>
    public static Point2D operator +(in Point2D left, in Point2D right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }

    /// <summary>
    /// Subtract two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Point2D"/>.</param>
    /// <param name="right">A <see cref="Point2D"/>.</param>
    /// <returns>The difference of the vectors.</returns>
    public static Point2D operator -(in Point2D left, in Point2D right)
    {
        return new(left.X - right.X, left.Y - right.Y);
    }

    /// <summary>
    /// Multiply two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Point2D"/>.</param>
    /// <param name="right">A <see cref="Point2D"/>.</param>
    /// <returns>The product of the vectors.</returns>
    public static Point2D operator *(in Point2D left, in Point2D right)
    {
        return new(left.X * right.X, left.Y * right.Y);
    }

    /// <summary>
    /// Divide two vectors.
    /// </summary>
    /// <param name="left">A <see cref="Point2D"/>.</param>
    /// <param name="right">A <see cref="Point2D"/>.</param>
    /// <returns>The quotient of the vectors.</returns>
    public static Point2D operator /(in Point2D left, in Point2D right)
    {
        return new(left.X / right.X, left.Y / right.Y);
    }

    /// <summary>
    /// Add a scalar to a <see cref="Point2D"/>.
    /// </summary>
    /// <param name="vector">A <see cref="Point2D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The sum of the <see cref="Point2D"/> and the scalar.</returns>
    public static Point2D operator +(in Point2D vector, in float scalar)
    {
        return new(vector.X + scalar, vector.Y + scalar);
    }

    /// <summary>
    /// Subtract a scalar from a <see cref="Point2D"/>.
    /// </summary>
    /// <param name="vector">A <see cref="Point2D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The difference of the <see cref="Point2D"/> and the scalar.</returns>
    public static Point2D operator -(in Point2D vector, in float scalar)
    {
        return new(vector.X - scalar, vector.Y - scalar);
    }

    /// <summary>
    /// Multiply a <see cref="Point2D"/> by a scalar.
    /// </summary>
    /// <param name="vector">A <see cref="Point2D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The product of the <see cref="Point2D"/> and the scalar.</returns>
    public static Point2D operator *(in Point2D vector, in float scalar)
    {
        return new(vector.X * scalar, vector.Y * scalar);
    }

    /// <summary>
    /// Divide a <see cref="Point2D"/> by a scalar.
    /// </summary>
    /// <param name="vector">A <see cref="Point2D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The quotient of the <see cref="Point2D"/> and the scalar.</returns>
    public static Point2D operator /(in Point2D vector, in float scalar)
    {
        return new(vector.X / scalar, vector.Y / scalar);
    }

    /// <summary>
    /// Get the distance from <see cref="Point2D"/> <paramref name="a"/> to <see cref="Point2D"/> <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The starting <see cref="Point2D"/>.</param>
    /// <param name="b">The target <see cref="Point2D"/>.</param>
    /// <returns>The distance between the two points.</returns>
    public static float Distance(Point2D a, Point2D b)
    {
        return Distance(a.X, a.Y, b.X, b.Y);
    }

    /// <summary>
    /// Get the distance between two points.
    /// </summary>
    /// <param name="ax">The starting point's X-coordinate.</param>
    /// <param name="ay">The starting point's Y-coordinate.</param>
    /// <param name="bx">The target point's X-coordinate.</param>
    /// <param name="by">The target point's Y-coordinate.</param>
    /// <returns>The distance between the two points.</returns>
    public static float Distance(in float ax, in float ay, in float bx, in float by)
    {
        return MathF.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay));
    }

    /// <summary>
    /// Get the direction from <see cref="Point2D"/> <paramref name="a"/> to <see cref="Point2D"/> <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The starting <see cref="Point2D"/>.</param>
    /// <param name="b">The target <see cref="Point2D"/>.</param>
    /// <returns>The direction from <see cref="Point2D"/> <paramref name="a"/> to <see cref="Point2D"/> <paramref name="b"/>.</returns>
    public static float Heading(Point2D a, Point2D b)
    {
        return Heading(a.X, a.Y, b.X, b.Y);
    }

    /// <summary>
    /// Get the heading direction between two points.
    /// </summary>
    /// <param name="ax">The starting point's X-coordinate.</param>
    /// <param name="ay">The starting point's Y-coordinate.</param>
    /// <param name="bx">The target point's X-coordinate.</param>
    /// <param name="by">The target point's Y-coordinate.</param>
    /// <returns>The from the first point to the second point.</returns>
    public static float Heading(in float ax, in float ay, in float bx, in float by)
    {
        return (float) (MathF.Atan2(by - ay, bx - ax) * (180.0F / Math.PI));
    }
}