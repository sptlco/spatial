// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Helpers;

/// <summary>
/// Tests for <see cref="Cipher"/>.
/// </summary>
public class CipherHelperTests
{
    /// <summary>
    /// Generate a keystream of the requested size.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGenerateKeystreamReturnsRequestedSize()
    {
        var keystream = Cipher.GenerateKeystream(48);

        Assert.Equal(48, keystream.Length);
    }

    /// <summary>
    /// Generate a keystream deterministically for a given seed.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGenerateKeystreamIsDeterministicForSeed()
    {
        var first = Cipher.GenerateKeystream(32, 42);
        var second = Cipher.GenerateKeystream(32, 42);

        Assert.Equal(first, second);
    }

    /// <summary>
    /// Generate different keystreams for different seeds.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGenerateKeystreamDiffersForDifferentSeeds()
    {
        var first = Cipher.GenerateKeystream(32, 1);
        var second = Cipher.GenerateKeystream(32, 2);

        Assert.NotEqual(first, second);
    }

    /// <summary>
    /// Generate a keystream whose length is not a multiple of the hash block size.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGenerateKeystreamHandlesPartialBlock()
    {
        var keystream = Cipher.GenerateKeystream(40);

        Assert.Equal(40, keystream.Length);
    }

    /// <summary>
    /// Generate an empty keystream when size is zero.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGenerateKeystreamWithZeroSize()
    {
        var keystream = Cipher.GenerateKeystream(0);

        Assert.Empty(keystream);
    }
}