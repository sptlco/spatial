// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Mathematics;

/// <summary>
/// A point in two-dimensional space.
/// </summary>
public readonly record struct Point2D
{
    private readonly Tensor _tensor;

    /// <summary>
    /// Create a new <see cref="Point2D"/>,
    /// </summary>
    /// <param name="scalar"></param>
    public Point2D(float scalar = 0.0F)
    {
        _tensor = Tensor.Create([2], _ => scalar);
    }

    /// <summary>
    /// Create a new <see cref="Point2D"/>.
    /// </summary>
    /// <param name="x">The point's X-component.</param>
    /// <param name="y">The point's Y-component.</param>
    public Point2D(float x, float y)
    {
        _tensor = Tensor.Zero([2]);

        _tensor[0] = x;
        _tensor[1] = y;
    }

    /// <summary>
    /// The point's X-component.
    /// </summary>
    public float X
    {
        get => _tensor[0];
        set => _tensor[0] = value;
    }

    /// <summary>
    /// The point's Y-component.
    /// </summary>
    public float Y
    {
        get => _tensor[1];
        set => _tensor[1] = value;
    }

    /// <summary>
    /// Add two points.
    /// </summary>
    /// <param name="left">A <see cref="Point2D"/>.</param>
    /// <param name="right">A <see cref="Point2D"/>.</param>
    /// <returns>The sum of the points.</returns>
    public static Point2D operator +(in Point2D left, in Point2D right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }

    /// <summary>
    /// Subtract two points.
    /// </summary>
    /// <param name="left">A <see cref="Point2D"/>.</param>
    /// <param name="right">A <see cref="Point2D"/>.</param>
    /// <returns>The difference of the points.</returns>
    public static Point2D operator -(in Point2D left, in Point2D right)
    {
        return new(left.X - right.X, left.Y - right.Y);
    }

    /// <summary>
    /// Multiply two points.
    /// </summary>
    /// <param name="left">A <see cref="Point2D"/>.</param>
    /// <param name="right">A <see cref="Point2D"/>.</param>
    /// <returns>The product of the points.</returns>
    public static Point2D operator *(in Point2D left, in Point2D right)
    {
        return new(left.X * right.X, left.Y * right.Y);
    }

    /// <summary>
    /// Divide two points.
    /// </summary>
    /// <param name="left">A <see cref="Point2D"/>.</param>
    /// <param name="right">A <see cref="Point2D"/>.</param>
    /// <returns>The quotient of the points.</returns>
    public static Point2D operator /(in Point2D left, in Point2D right)
    {
        return new(left.X / right.X, left.Y / right.Y);
    }

    /// <summary>
    /// Add a scalar to a <see cref="Point2D"/>.
    /// </summary>
    /// <param name="point">A <see cref="Point2D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The sum of the <see cref="Point2D"/> and the scalar.</returns>
    public static Point2D operator +(in Point2D point, in float scalar)
    {
        return new(point.X + scalar, point.Y + scalar);
    }

    /// <summary>
    /// Subtract a scalar from a <see cref="Point2D"/>.
    /// </summary>
    /// <param name="point">A <see cref="Point2D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The difference of the <see cref="Point2D"/> and the scalar.</returns>
    public static Point2D operator -(in Point2D point, in float scalar)
    {
        return new(point.X - scalar, point.Y - scalar);
    }

    /// <summary>
    /// Multiply a <see cref="Point2D"/> by a scalar.
    /// </summary>
    /// <param name="point">A <see cref="Point2D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The product of the <see cref="Point2D"/> and the scalar.</returns>
    public static Point2D operator *(in Point2D point, in float scalar)
    {
        return new(point.X * scalar, point.Y * scalar);
    }

    /// <summary>
    /// Divide a <see cref="Point2D"/> by a scalar.
    /// </summary>
    /// <param name="point">A <see cref="Point2D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The quotient of the <see cref="Point2D"/> and the scalar.</returns>
    public static Point2D operator /(in Point2D point, in float scalar)
    {
        return new(point.X / scalar, point.Y / scalar);
    }
}