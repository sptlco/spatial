// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// Tests for <see cref="Managed"/>.
/// </summary>
public class ManagedTests
{
    /// <summary>
    /// Allocate unmanaged memory.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAllocate()
    {
        var reference = Managed<int>.Allocate();

        Assert.NotNull(reference);
        Assert.Equal(0, reference.Value);
    }

    /// <summary>
    /// Free allocated memory.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFree()
    {
        var reference = Managed<int>.Allocate();

        Managed<int>.Free(reference);
    }

    /// <summary>
    /// Copy unmanaged memory.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestCopy()
    {
        var ref1 = Managed<int>.Allocate();
        var ref2 = Managed<int>.Allocate();

        ref2.Value = 5;

        Managed<int>.Copy(ref2, 0, ref1, 0, 1);

        Assert.Equal(5, ref1.Value);
    }
}
