// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// Tests for a <see cref="Reference{T}"/>.
/// </summary>
public class ReferenceTests
{
    /// <summary>
    /// Test the <see cref="Reference{T}"/> indexer.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestIndexer()
    {
        var reference = Managed<int>.Allocate();

        reference[0] = 5;

        Assert.Equal(5, reference[0]);
    }

    /// <summary>
    /// Dispose of a <see cref="Reference{T}"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestDispose()
    {
        Managed<int>.Allocate().Dispose();
    }
}
