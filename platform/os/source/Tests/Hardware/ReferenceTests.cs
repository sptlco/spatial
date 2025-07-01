// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Hardware;

/// <summary>
/// Unit tests for <see cref="Reference{T}"/>.
/// </summary>
public unsafe class ReferenceTests
{
    /// <summary>
    /// Test <see cref="Reference{T}.Pointer"/>.
    /// </summary>
    [Fact]
    public void TestPointer()
    {
        using var memory = Managed<int>.Allocate(5);
        var pointer = memory.Pointer;

        Assert.NotEqual(IntPtr.Zero, (IntPtr) pointer);
    }

    /// <summary>
    /// Test <see cref="Reference{T}.Value"/>.
    /// </summary>
    [Fact]
    public void TestValue()
    {
        using var memory = Managed<int>.Allocate(5);
        ref int value = ref memory.Value;

        value = 42;

        Assert.Equal(42, value);
    }

    /// <summary>
    /// Test <see cref="Reference{T}.this[int]"/>.
    /// </summary>
    [Fact]
    public void TestIndexer()
    {
        using var memory = Managed<int>.Allocate(5);
        ref int value = ref memory[2];

        value = 42;
        
        Assert.Equal(42, value);
    }

    /// <summary>
    /// Test <see cref="Reference{T}.Dispose"/>.
    /// </summary>
    [Fact]
    public void TestDispose()
    {
        Managed<int>.Allocate(5).Dispose();
    }
}