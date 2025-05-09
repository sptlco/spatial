// Copyright © Spatial. All rights reserved.

using Spatial.Extensions;
using System.IO;

namespace Ignite.Assets.Loaders;

/// <summary>
/// A loader for bitmaps.
/// </summary>
public static class BitmapLoader
{
    /// <summary>
    /// Load a <see cref="Bitmap"/>.
    /// </summary>
    /// <param name="root">The file's root directory.</param>
    /// <param name="path">The file's path, relative to <paramref name="root"/>.</param>
    /// <returns>A <see cref="Bitmap"/>.</returns>
    public static Bitmap Load(string root, string path)
    {
        var data = File.ReadAllBytes(path);
        var bitmap = new Bitmap 
        {
            Path = path,
            Name = Path.GetRelativePath(root, path),
            Hash = data.ToMD5()
        };

        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        var file = new BITMAPFILEHEADER
        {
            bfType = reader.ReadUInt16(),
            bfSize = reader.ReadUInt32(),
            bfReserved1 = reader.ReadUInt16(),
            bfReserved2 = reader.ReadUInt16(),
            bfOffBits = reader.ReadUInt32()
        };

        var info = new BITMAPINFOHEADER
        {
            biSize = reader.ReadUInt32(),
            biWidth = reader.ReadInt32(),
            biHeight = reader.ReadInt32(),
            biPlanes = reader.ReadUInt16(),
            biBitCount = reader.ReadUInt16(),
            biCompression = reader.ReadUInt32(),
            biSizeImage = reader.ReadUInt32(),
            biXPelsPerMeter = reader.ReadInt32(),
            biYPelsPerMeter = reader.ReadInt32(),
            biClrUsed = reader.ReadUInt32(),
            biClrImportant = reader.ReadUInt32()
        };

        for (var i = 0; i < info.biClrUsed; i++)
        {
            var b = reader.ReadByte();
            var g = reader.ReadByte();
            var r = reader.ReadByte();
            var a = reader.ReadByte();
            
            bitmap.Colors.Add(Color.FromRGB(r, g, b));
        }

        bitmap.Width = info.biWidth;
        bitmap.Height = info.biHeight;
        bitmap.Pixels = reader.ReadBytes((int) info.biSizeImage);

        return bitmap;
    }
}