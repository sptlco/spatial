// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;
using System;

namespace Ignite.Components;

/// <summary>
/// An area inhabited by mobs.
/// </summary>
/// <param name="Shape">The region's <see cref="Components.Shape"/>.</param>
/// <param name="X">The region's X-coordinate.</param>
/// <param name="Y">The region's Y-coordinate.</param>
/// <param name="Width">The width of the <see cref="Region"/>.</param>
/// <param name="Height">The height of the <see cref="Region"/>.</param>
/// <param name="Radius">The radius of the <see cref="Region"/>.</param>
/// <param name="Rotation">The rotation of the <see cref="Region"/>.</param>
public record struct Region(
    Shape Shape = Shape.Rectangle,
    float X = 0F,
    float Y = 0F,
    float Width = 0F,
    float Height = 0F,
    float Radius = 0F,
    float Rotation = 0F) : IComponent
{
    /// <summary>
    /// The sine of the rotation of the <see cref="Rectangle"/>.
    /// </summary>
    public readonly float Sine => (float) Math.Sin(Rotation * Math.PI / 180);

    /// <summary>
    /// The sine of the rotation of the <see cref="Rectangle"/>.
    /// </summary>
    public readonly float Cosine => (float) Math.Cos(Rotation * Math.PI / 180);
}

/// <summary>
/// Specifies the shape of a <see cref="Region"/>.
/// </summary>
public enum Shape
{
    Circle,
    Rectangle
}