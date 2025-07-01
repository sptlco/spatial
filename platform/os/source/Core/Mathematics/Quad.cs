// Copyright © Spatial Corporation. All rights reserved.

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
    float Bottom = 0F);