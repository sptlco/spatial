// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Encodes and decodes keystream ciphers.
/// </summary>
public abstract class Transformer
{
    /// <summary>
    /// The transformer's keystream.
    /// </summary>
    protected readonly byte[] _keystream;

    /// <summary>
    /// Create a new <see cref="Transformer"/>.
    /// </summary>
    /// <param name="keystream">The transformer's keystream.</param>
    public Transformer(byte[] keystream)
    {
        _keystream = keystream;
    }

    /// <summary>
    /// The transformer's keystream.
    /// </summary>
    public byte[] Keystream => _keystream;

    /// <summary>
    /// Encode a cipher.
    /// </summary>
    /// <param name="cipher">The cipher to encode.</param>
    /// <param name="offset">The location within <paramref name="cipher"/> to begin the transformation.</param>
    /// <param name="count">The number of bytes to encode.</param>
    /// <param name="keys">A <see cref="KeyPair"/>.</param>
    public virtual void Encode(byte[] cipher, int offset, int count, KeyPair keys) {}

    /// <summary>
    /// Decode a cipher.
    /// </summary>
    /// <param name="cipher">The cipher to decode.</param>
    /// <param name="offset">The location within <paramref name="cipher"/> to begin the transformation.</param>
    /// <param name="count">The number of bytes to decode.</param>
    /// <param name="keys">A <see cref="KeyPair"/>.</param>
    public virtual void Decode(byte[] cipher, int offset, int count, KeyPair keys) {}
}

/// <summary>
/// A pair of <see cref="Transformer"/> keys.
/// </summary>
public ref struct KeyPair
{
    private ref int _encoder;
    private ref int _decoder;

    /// <summary>
    /// Create a new <see cref="KeyPair"/>.
    /// </summary>
    /// <param name="encoder">A key for encoding.</param>
    /// <param name="decoder">A key for decoding.</param>
    public KeyPair(ref int encoder, ref int decoder)
    {
        _encoder = ref encoder;
        _decoder = ref decoder;
    }

    /// <summary>
    /// A key for encoding.
    /// </summary>
    public ref int Encoder => ref _encoder;

    /// <summary>
    /// A key for decoding.
    /// </summary>
    public ref int Decoder => ref _decoder;
}