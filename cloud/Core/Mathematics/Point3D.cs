// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Mathematics;

/// <summary>
/// A point in three-dimensional space.
/// </summary>
public class Point3D
{
    private readonly Tensor _tensor;

    /// <summary>
    /// Get a <see cref="Point3D"/> at the origin.
    /// </summary>
    public static Point3D Zero => new Point3D();

    /// <summary>
    /// Create a new <see cref="Point2D"/>,
    /// </summary>
    /// <param name="scalar"></param>
    public Point3D(double scalar = 0.0F)
    {
        _tensor = Tensor.Create([3], _ => scalar);
    }

    /// <summary>
    /// Create a new <see cref="Point2D"/>.
    /// </summary>
    /// <param name="x">The point's X-component.</param>
    /// <param name="y">The point's Y-component.</param>
    /// <param name="z">The point's Z-component.</param>
    public Point3D(double x, double y, double z)
    {
        _tensor = Tensor.Zero([3]);

        _tensor[0] = x;
        _tensor[1] = y;
        _tensor[2] = z;
    }

    /// <summary>
    /// The point's X-component.
    /// </summary>
    public double X
    {
        get => _tensor[0];
        set => _tensor[0] = value;
    }

    /// <summary>
    /// The point's Y-component.
    /// </summary>
    public double Y
    {
        get => _tensor[1];
        set => _tensor[1] = value;
    }

    /// <summary>
    /// The point's Z-component.
    /// </summary>
    public double Z
    {
        get => _tensor[2];
        set => _tensor[2] = value;
    }

    /// <summary>
    /// Add two points.
    /// </summary>
    /// <param name="left">A <see cref="Point3D"/>.</param>
    /// <param name="right">A <see cref="Point3D"/>.</param>
    /// <returns>The sum of the points.</returns>
    public static Point3D operator +(in Point3D left, in Point3D right)
    {
        return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    /// <summary>
    /// Subtract two points.
    /// </summary>
    /// <param name="left">A <see cref="Point3D"/>.</param>
    /// <param name="right">A <see cref="Point3D"/>.</param>
    /// <returns>The difference of the points.</returns>
    public static Point3D operator -(in Point3D left, in Point3D right)
    {
        return new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    /// <summary>
    /// Multiply two points.
    /// </summary>
    /// <param name="left">A <see cref="Point3D"/>.</param>
    /// <param name="right">A <see cref="Point3D"/>.</param>
    /// <returns>The product of the points.</returns>
    public static Point3D operator *(in Point3D left, in Point3D right)
    {
        return new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    /// <summary>
    /// Divide two points.
    /// </summary>
    /// <param name="left">A <see cref="Point3D"/>.</param>
    /// <param name="right">A <see cref="Point3D"/>.</param>
    /// <returns>The quotient of the points.</returns>
    public static Point3D operator /(in Point3D left, in Point3D right)
    {
        return new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    /// <summary>
    /// Add a scalar to a <see cref="Point3D"/>.
    /// </summary>
    /// <param name="point">A <see cref="Point3D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The sum of the <see cref="Point3D"/> and the scalar.</returns>
    public static Point3D operator +(in Point3D point, in double scalar)
    {
        return new(point.X + scalar, point.Y + scalar, point.Z + scalar);
    }

    /// <summary>
    /// Subtract a scalar from a <see cref="Point3D"/>.
    /// </summary>
    /// <param name="point">A <see cref="Point3D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The difference of the <see cref="Point3D"/> and the scalar.</returns>
    public static Point3D operator -(in Point3D point, in double scalar)
    {
        return new(point.X - scalar, point.Y - scalar, point.Z - scalar);
    }

    /// <summary>
    /// Multiply a <see cref="Point3D"/> by a scalar.
    /// </summary>
    /// <param name="point">A <see cref="Point3D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The product of the <see cref="Point3D"/> and the scalar.</returns>
    public static Point3D operator *(in Point3D point, in double scalar)
    {
        return new(point.X * scalar, point.Y * scalar, point.Y * scalar);
    }

    /// <summary>
    /// Divide a <see cref="Point3D"/> by a scalar.
    /// </summary>
    /// <param name="point">A <see cref="Point3D"/>.</param>
    /// <param name="scalar">A scalar value.</param>
    /// <returns>The quotient of the <see cref="Point3D"/> and the scalar.</returns>
    public static Point3D operator /(in Point3D point, in double scalar)
    {
        return new(point.X / scalar, point.Y / scalar, point.Z / scalar);
    }
}