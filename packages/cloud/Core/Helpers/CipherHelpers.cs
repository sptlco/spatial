// Copyright © Spatial Corporation. All rights reserved.

using System.Security.Cryptography;

namespace Spatial.Helpers;

/// <summary>
/// Helper methods for ciphers.
/// </summary>
public static class Cipher
{
    /// <summary>
    /// Generate a cryptographically strong keystream.
    /// </summary>
    /// <param name="size">The size of the keystream.</param>
    /// <param name="seed">An optional seed value for generation.</param>
    /// <returns>A cryptographyically strong keystream.</returns>
    public static byte[] GenerateKeystream(int size, ushort seed = 0)
    {
        var state = BitConverter.GetBytes(seed);
        var keystream = new byte[size];

        for (var i = 0; i < keystream.Length; i += 32)
        {
            Array.Copy(state = SHA256.HashData(state), 0, keystream, i, Math.Min(32, keystream.Length - i));
        }

        return keystream;
    }
}