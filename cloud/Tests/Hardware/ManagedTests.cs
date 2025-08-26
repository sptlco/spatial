// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Hardware;

/// <summary>
/// Unit tests for <see cref="Managed{T}"/>.
/// </summary>
public class ManagedTests
{
    /// <summary>
    /// Test <see cref="Managed{T}.Allocate"/>.
    /// </summary>
    [Fact]
    public void TestAllocate()
    {
        Managed<int>.Allocate(5);
    }

    /// <summary>
    /// Test <see cref="Managed{T}.Free"/>.
    /// </summary>
    [Fact]
    public void TestFree()
    {
        Managed<int>.Free(Managed<int>.Allocate(5));
    }

    /// <summary>
    /// Test <see cref="Managed{T}.Copy"/>.
    /// </summary>
    [Fact]
    public void TestCopy()
    {
        var numbers1 = Managed<int>.Allocate(3);
        var numbers2 = Managed<int>.Allocate(3);

        numbers2[1] = 5;

        Managed<int>.Copy(numbers2, 1, numbers1, 0, 1);
        Assert.Equal(5, numbers1[0]);
    }
}