// Copyright © Spatial. All rights reserved.

namespace Spatial.Mathematics;

/// <summary>
/// A structure with 4 sides, commonly referred to as a quadrant.
/// </summary>
/// <param name="Left">The left side's coordinate.</param>
/// <param name="Top">The top side's coordinate.</param>
/// <param name="Right">The right side's coordinate.</param>
/// <param name="Bottom">The bottom side's coordinate.</param>
public record struct Quad(
    float Left = 0F,
    float Top = 0F,
    float Right = 0F,
    float Bottom = 0F)
{
    /// <summary>
    /// Get whether or not the <see cref="Quad"/> intersects with a circle.
    /// </summary>
    /// <param name="x">The circle's X-coordinate.</param>
    /// <param name="y">The circle's Y-coordinate.</param>
    /// <param name="radius">The circle's radius.</param>
    /// <returns>Whether or not the two objects intersect.</returns>
    public readonly bool Intersects(in float x, in float y, in float radius)
    {
        var bx = Math.Clamp(x, Left, Right);
        var by = Math.Clamp(y, Bottom, Top);

        return Point2D.Distance(x, y, bx, by) <= radius;
    }  
}