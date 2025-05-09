// Copyright © Spatial. All rights reserved.

using System;

namespace Ignite.Assets;

/// <summary>
/// A map of blocked positions.
/// </summary>
public class Blockmap : Asset
{
    private readonly int _width;
    private readonly int _height;
    private readonly byte[] _blocks;

    /// <summary>
    /// Create a new <see cref="Blockmap"/>.
    /// </summary>
    /// <param name="width">The width of the <see cref="Blockmap"/>.</param>
    /// <param name="height">The height of the <see cref="Blockmap"/>.</param>
    /// <param name="blocks">An array of blocks.</param>
    public Blockmap(int width, int height, byte[] blocks)
    {
        _width = width;
        _height = height;
        _blocks = blocks;
    }

    /// <summary>
    /// The width of the <see cref="Blockmap"/>.
    /// </summary>
    public int Width => _width * 8;

    /// <summary>
    /// The height of the <see cref="Blockmap"/>.
    /// </summary>
    public int Height => _height;

    /// <summary>
    /// Get a position in the <see cref="Blockmap"/>.
    /// </summary>
    /// <param name="x">An X-coordinate in units.</param>
    /// <param name="y">A Y-coordinate in units.</param>
    /// <returns>A position.</returns>
    public bool this[float x, float y]
    {
        get
        {
            var bx = (int) (x / 6.25F);
            var by = (int) (y / 6.25F);

            return this[bx, by];
        }
    }

    /// <summary>
    /// Get a position in the <see cref="Blockmap"/>.
    /// </summary>
    /// <param name="x">An X-coordinate.</param>
    /// <param name="y">A Y-coordinate.</param>
    /// <returns>A position.</returns>
    public bool this[int x, int y]
    {
        get {
            x = Math.Clamp(x, 0, Width - 1);
            y = Math.Clamp(y, 0, Height - 1);

            var index = y * _width + x / 8;
            var bit = x % 8;

            return (_blocks[index] & 1 << bit) != 0;
        }
    }
}