// Copyright © Spatial Corporation. All rights reserved.

using System.Security.Cryptography;

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for <see cref="byte"/> arrays.
/// </summary>
public static class ByteArrayExtensions
{
    /// <summary>
    /// Get the MD5 hash of a byte array.
    /// </summary>
    /// <param name="data">The array to get the hash for.</param>
    /// <returns>The hash of the array.</returns>
    public static string ToMD5(this byte[] data)
    {
        return Convert.ToHexStringLower(MD5.HashData(data));
    }
}