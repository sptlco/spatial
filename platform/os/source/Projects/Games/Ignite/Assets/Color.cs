// Copyright © Spatial. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ignite.Assets;

/// <summary>
/// An 8-bit RGBA color.
/// </summary>
public struct Color
{
    /// <summary>
    /// The <see cref="Color"/> black.
    /// </summary>
    public static Color Black => Colors[0];

    /// <summary>
    /// The <see cref="Color"/> white.
    /// </summary>
    public static Color White => Colors[1];
    
    /// <summary>
    /// The color's red value.
    /// </summary>
    public readonly float Red;

    /// <summary>
    /// The color's green value.
    /// </summary>
    public readonly float Green;

    /// <summary>
    /// The color's blue value.
    /// </summary>
    public readonly float Blue;

    private static IList<Color> Colors = [
        new(0, 0, 0),
        new(1F, 1, 1)
    ];

    /// <summary>
    /// Create a new <see cref="Color"/>.
    /// </summary>
    /// <param name="red">The color's red value.</param>
    /// <param name="green">The color's green value.</param>
    /// <param name="blue">The color's blue value.</param>
    private Color(float red, float green, float blue)
    {
        Red = Math.Clamp(red, 0, 1);
        Green = Math.Clamp(green, 0, 1);
        Blue = Math.Clamp(blue, 0, 1);
    }

    /// <summary>
    /// Create a new <see cref="Color"/>.
    /// </summary>
    /// <param name="red">A red value.</param>
    /// <param name="green">A green value.</param>
    /// <param name="blue">A blue value.</param>
    /// <returns>A <see cref="Color"/>.</returns>
    public static Color FromRGB(float red, float green, float blue)
    {
        red = Math.Clamp(red, 0, 1);
        green = Math.Clamp(green, 0, 1);
        blue = Math.Clamp(blue, 0, 1);

        Color? color = Colors.FirstOrDefault(c => 
            c.Red == red && 
            c.Green == green && 
            c.Blue == blue);

        return color ?? new Color(red, green, blue);
    }

    /// <summary>
    /// Create a new <see cref="Color"/>.
    /// </summary>
    /// <param name="red">A red value.</param>
    /// <param name="green">A green value.</param>
    /// <param name="blue">A blue value.</param>
    /// <returns>A <see cref="Color"/>.</returns>
    public static Color FromRGB(byte red, byte green, byte blue)
    {
        return FromRGB(red / 255F, green / 255F, blue / 255F);
    }
}