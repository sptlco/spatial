// Copyright © Spatial. All rights reserved.

using Spatial.Mathematics;

namespace Ignite.Assets;

/// <summary>
/// A blocked area.
/// </summary>
public class Blockade : Asset
{
    /// <summary>
    /// The blockade's identifier.
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The start position of the <see cref="Blockade"/>.
    /// </summary>
    public Point2D Start { get; set; }

    /// <summary>
    /// The end position of the <see cref="Blockade"/>.
    /// </summary>
    public Point2D End { get; set; }

    /// <summary>
    /// The size of the <see cref="Blockade"/> in memory.
    /// </summary>
    public uint Size { get; set; }

    /// <summary>
    /// The address of the <see cref="Blockade"/> in memory.
    /// </summary>
    public uint Address { get; set; }

    /// <summary>
    /// The <see cref="Blockade"/> in memory.
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    /// The blocks of the <see cref="Blockade"/>.
    /// </summary>
    public Blockmap Blocks { get; set; }

    /// <summary>
    /// The width of the <see cref="Blockade"/>.
    /// </summary>
    public int Width => (int) (End.X - Start.X + 1);

    /// <summary>
    /// The height of the <see cref="Blockade"/>.
    /// </summary>
    public int Height => (int) (End.Y - Start.Y + 1);
}