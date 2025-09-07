// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking.Helpers;

/// <summary>
/// Helper methods for ciphers.
/// </summary>
public static class Cipher
{
    /// <summary>
    /// Transform a <see cref="Cipher"/>.
    /// </summary>
    /// <param name="cipher">The <see cref="Cipher"/> to transform.</param>
    /// <param name="offset">The location within <paramref name="cipher"/> to begin encrypting.</param>
    /// <param name="count">The number of bytes to encrypt.</param>
    /// <param name="keystream">A keystream table.</param>
    /// <param name="key">A positional index within <paramref name="keystream"/>.</param>
    public static void Transform(byte[] cipher, int offset, int count, byte[] keystream, ref int key)
    {
        for (var i = 0; i < count; i++)
        {
            int current, update;

            do
            {
                current = Volatile.Read(ref key);
                update = (ushort) ((current + 1) % keystream.Length);
            }
            while (Interlocked.CompareExchange(ref key, update, current) != current);

            cipher[offset + i] ^= keystream[current];
        }
    }
}