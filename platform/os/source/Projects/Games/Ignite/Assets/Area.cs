// Copyright © Spatial. All rights reserved.

using Spatial.Mathematics;
using System;

namespace Ignite.Assets;

/// <summary>
/// An area of a map.
/// </summary>
public abstract class Area : Asset
{
    /// <summary>
    /// The area's identifier.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The position of the <see cref="Circle"/>.
    /// </summary>
    public Point2D Position { get; set; }

    /// <summary>
    /// Get whether or not a <see cref="Point2D"/> is within the <see cref="Area"/>. 
    /// </summary>
    /// <param name="point">A <see cref="Point2D"/>.</param>
    /// <returns>Whether or not the <see cref="Point2D"/> is within the <see cref="Area"/>.</returns>
    public bool Contains(Point2D point) => Contains(point.X, point.Y);

    /// <summary>
    /// Get whether or not a <see cref="Point2D"/> is within the <see cref="Area"/>. 
    /// </summary>
    /// <param name="x">An X-coordinate.</param>
    /// <param name="y">A Y-coordinate.</param>
    /// <returns>Whether or not the <see cref="Point2D"/> is within the <see cref="Area"/>.</returns>
    public abstract bool Contains(in float x, in float y);
}

/// <summary>
/// A circular <see cref="Area"/>.
/// </summary>
public class Circle : Area
{
    /// <summary>
    /// The radius of the <see cref="Circle"/>.
    /// </summary>
    public float Radius { get; set; }

    /// <summary>
    /// Get whether or not a <see cref="Point2D"/> is within a <see cref="Circle"/>.
    /// </summary>
    /// <param name="cx">The circle's X-coordinate.</param>
    /// <param name="cy">The circle's Y-coordinate.</param>
    /// <param name="radius">The circle's radius.</param>
    /// <param name="px">The point's X-coordinate.</param>
    /// <param name="py">The point's Y-coordinate.</param>
    /// <returns></returns>
    public static bool Contains(in float cx, in float cy, in float radius, in float px, in float py)
    {
        return Point2D.Distance(px, py, cx, cy) <= radius;
    }

    /// <summary>
    /// Get whether or not a <see cref="Point2D"/> is within the <see cref="Circle"/>.
    /// </summary>
    /// <param name="x">The point's X-coordinate.</param>
    /// <param name="y">The point's Y-coordinate.</param>
    /// <returns>Whether or not the <see cref="Point2D"/> is within the <see cref="Circle"/>.</returns>
    public override bool Contains(in float x, in float y)
    {
        return Contains(Position.X, Position.Y, Radius, x, y);
    }
}

/// <summary>
/// A rectangular <see cref="Area"/>.
/// </summary>
public class Rectangle : Area
{
    /// <summary>
    /// The size of the <see cref="Rectangle"/>.
    /// </summary>
    public Point2D Size { get; set; }

    /// <summary>
    /// The rotation of the <see cref="Rectangle"/>.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    /// The sine of the rotation of the <see cref="Rectangle"/>.
    /// </summary>
    public float Sine => (float) Math.Sin(Rotation * Math.PI / 180);

    /// <summary>
    /// The sine of the rotation of the <see cref="Rectangle"/>.
    /// </summary>
    public float Cosine => (float) Math.Cos(Rotation * Math.PI / 180);

    /// <summary>
    /// Get whether or not a <see cref="Point2D"/> is within a <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="cx">The rectangle's X-coordinate.</param>
    /// <param name="cy">The rectangle's Y-coordinate.</param>
    /// <param name="width">The width of the <see cref="Rectangle"/>.</param>
    /// <param name="height">The height of the <see cref="Rectangle"/>.</param>
    /// <param name="rotation">The rectangle's rotation.</param>
    /// <param name="x">The point's X-coordinate.</param>
    /// <param name="y">The point's Y-coordinate.</param>
    /// <returns></returns>
    public static bool Contains(in float cx, in float cy, in float width, in float height, in float rotation, in float x, in float y)
    {
        var dx = x - cx;
        var dy = y - cy;

        var radians = rotation * (float) (Math.PI / 180);
        var cos = (float) Math.Cos(radians);
        var sin = (float) Math.Sin(radians);

        var rx = dx * cos + dy * sin;
        var ry = -dx * sin + dy * cos;

        return Math.Abs(rx) <= (width / 2) && Math.Abs(ry) <= (height / 2);
    }

    /// <summary>
    /// Get whether or not a <see cref="Point2D"/> is within the <see cref="Circle"/>.
    /// </summary>
    /// <param name="point">A <see cref="Point2D"/>.</param>
    /// <returns>Whether or not the <see cref="Point2D"/> is within the <see cref="Circle"/>.</returns>
    public override bool Contains(in float x, in float y)
    {
        return Contains(Position.X, Position.Y, Size.X, Size.Y, Rotation, x, y);
    }
}
