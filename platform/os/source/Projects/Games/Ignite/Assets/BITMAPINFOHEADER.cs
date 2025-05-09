// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets;

/// <summary>
/// Information about the dimensions and color format of 
/// a device-independent bitmap (DIB).
/// </summary>
public struct BITMAPINFOHEADER
{
    /// <summary>
    /// Specifies the number of bytes required by the structure. 
    /// This value does not include the size of the color table or 
    /// the size of the color masks, if they are appended to the 
    /// end of structure.
    /// </summary>
    public uint biSize;

    /// <summary>
    /// Specifies the width of the bitmap, in pixels.
    /// </summary>
    public int biWidth;

    /// <summary>
    /// Specifies the height of the bitmap, in pixels.
    /// </summary>
    public int biHeight;

    /// <summary>
    /// Specifies the number of planes for the target device. 
    /// This value must be set to 1.
    /// </summary>
    public ushort biPlanes;

    /// <summary>
    /// Specifies the number of bits per pixel (bpp). 
    /// For uncompressed formats, this value is the average number 
    /// of bits per pixel. For compressed formats, this value is 
    /// the implied bit depth of the uncompressed image, after 
    /// the image has been decoded.
    /// </summary>
    public ushort biBitCount;

    /// <summary>
    /// For compressed video and YUV formats, this member is a FOURCC code, 
    /// specified as a DWORD in little-endian order. For example, YUYV video has 
    /// the FOURCC 'VYUY' or 0x56595559.
    /// </summary>
    public uint biCompression;

    /// <summary>
    /// Specifies the size, in bytes, of the image. 
    /// This can be set to 0 for uncompressed RGB bitmaps.
    /// </summary>
    public uint biSizeImage;

    /// <summary>
    /// Specifies the horizontal resolution, in pixels per meter, 
    /// of the target device for the bitmap.
    /// </summary>
    public int biXPelsPerMeter;

    /// <summary>
    /// Specifies the vertical resolution, in pixels per meter, 
    /// of the target device for the bitmap.
    /// </summary>
    public int biYPelsPerMeter;

    /// <summary>
    /// Specifies the number of color indices in the color table 
    /// that are actually used by the bitmap.
    /// </summary>
    public uint biClrUsed;

    /// <summary>
    /// Specifies the number of color indices that are considered important 
    /// for displaying the bitmap. If this value is zero, all colors are important.
    /// </summary>
    public uint biClrImportant;
}