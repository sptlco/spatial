// Copyright © Spatial. All rights reserved.

using System.Text;

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for <see cref="BinaryReader"/>.
/// </summary>
public static class BinaryReaderExtensions
{
    /// <summary>
    /// Read a <see cref="string"/>.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
    /// <param name="length">The length of the <see cref="string"/>.</param>
    /// <returns>A <see cref="string"/>.</returns>
    public static string ReadString(this BinaryReader reader, int length)
    {
        var offset = 0;
        var buffer = reader.ReadBytes(length);

        while (offset < length && buffer[offset] != 0x00)
        {
            offset++;
        }

        return length > 0 ? Encoding.UTF8.GetString(buffer, 0, offset) : string.Empty;
    }
}
