// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets;

/// <summary>
/// Information about the type, size, and layout of a 
/// file that contains a DIB.
/// </summary>
public struct BITMAPFILEHEADER
{
    /// <summary>
    /// The file type.
    /// </summary>
    public ushort bfType;

    /// <summary>
    /// The size, in bytes, of the bitmap file.
    /// </summary>
    public uint bfSize;

    /// <summary>
    /// Reserved; must be zero.
    /// </summary>
    public ushort bfReserved1;

    /// <summary>
    /// Reserved; must be zero.
    /// </summary>
    public ushort bfReserved2;

    /// <summary>
    /// The offset, in bytes, from the beginning of the <see cref="BITMAPFILEHEADER"/>
    /// structure to the bitmap bits.
    /// </summary>
    public uint bfOffBits;
}
