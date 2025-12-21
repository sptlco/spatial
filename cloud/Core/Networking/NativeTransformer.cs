// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Helpers;

namespace Spatial.Networking;

/// <summary>
/// A bi-directional transformer with a keystream size of 512 bytes.
/// </summary>
public class NativeTransformer : Transformer
{
    /// <summary>
    /// Create a new <see cref="NativeTransformer"/>.
    /// </summary>
    public NativeTransformer() : base(Cipher.GenerateKeystream(512)) { }

    /// <summary>
    /// Encode a cipher.
    /// </summary>
    /// <param name="cipher">The cipher to encode.</param>
    /// <param name="offset">The location within <paramref name="cipher"/> to begin the transformation.</param>
    /// <param name="count">The number of bytes to encode.</param>
    /// <param name="keys">A <see cref="KeyPair"/>.</param>
    public override void Encode(byte[] cipher, int offset, int count, KeyPair keys) => Transform(cipher, offset, count, ref keys.Encoder);

    /// <summary>
    /// Decode a cipher.
    /// </summary>
    /// <param name="cipher">The cipher to decode.</param>
    /// <param name="offset">The location within <paramref name="cipher"/> to begin the transformation.</param>
    /// <param name="count">The number of bytes to decode.</param>
    /// <param name="keys">A <see cref="KeyPair"/>.</param>
    public override void Decode(byte[] cipher, int offset, int count, KeyPair keys) => Transform(cipher, offset, count, ref keys.Decoder);

    private void Transform(byte[] cipher, int offset, int count, ref int key)
    {
        for (var i = 0; i < count; i++)
        {
            int current, update;

            do
            {
                current = Volatile.Read(ref key);
                update = (ushort) ((current + 1) % _keystream.Length);
            }
            while (Interlocked.CompareExchange(ref key, update, current) != current);

            cipher[offset + i] ^= _keystream[current];
        }
    }
}