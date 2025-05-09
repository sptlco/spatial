// Copyright © Spatial. All rights reserved.

using System;
using System.Collections.Generic;

namespace Ignite.Assets;

/// <summary>
/// An array of bits that specify the color of each pixel in 
/// a rectangular array of pixels (8-bit).
/// </summary>
public class Bitmap : Asset
{
    /// <summary>
    /// The width of the <see cref="Bitmap"/>.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The height of the <see cref="Bitmap"/>.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// The pixels stored in the <see cref="Bitmap"/>.
    /// </summary>
    public byte[] Pixels { get; set; }

    /// <summary>
    /// The colors used in the <see cref="Bitmap"/>.
    /// </summary>
    public IList<Color> Colors { get; set; } = [];
    
    /// <summary>
    /// Get the <see cref="Color"/> of a pixel in the <see cref="Bitmap"/>.
    /// </summary>
    /// <param name="x">An X-coordinate in units.</param>
    /// <param name="y">A Y-coordinate in units.</param>
    /// <returns>A <see cref="Color"/>.</returns>
    public Color this[float x, float y]
    {
        get
        {
            var bx = (int) (x / 6.25F);
            var by = (int) (y / 6.25F);

            return this[bx, by];
        }
    }

    /// <summary>
    /// Get the <see cref="Color"/> of a pixel in the <see cref="Bitmap"/>.
    /// </summary>
    /// <param name="x">An X-coordinate.</param>
    /// <param name="y">A Y-coordinate.</param>
    /// <returns>A <see cref="Color"/>.</returns>e
    public Color this[int x, int y]
    {
        get {
            x = Math.Clamp(x, 0, Width - 1);
            y = Math.Clamp(y, 0, Height - 1);

            var span = Math.Ceiling(8F * Width / 32F) * 4;
            var index = (int) span * y + x;

            return Colors[Pixels[index]];
        }
    }
}