// Copyright © Spatial Corporation. All rights reserved.

using System.Runtime.CompilerServices;

namespace Spatial;

/// <summary>
/// A pointer to a block of unmanaged memory.
/// </summary>
/// <typeparam name="T">The type of memory to reference.</typeparam>
public unsafe class Reference<T> : IDisposable where T : unmanaged
{
    private readonly T* _pointer;

    /// <summary>
    /// Create a new <see cref="Reference{T}"/>.
    /// </summary>
    /// <param name="pointer">A pointer to <see cref="Managed{T}"/>.</param>
    internal Reference(T* pointer)
    {
        _pointer = pointer;
    }

    /// <summary>
    /// Get a pointer to <see cref="Managed{T}"/>.
    /// </summary>
    public T* Pointer => _pointer;

    /// <summary>
    /// Get the value of the <see cref="Reference{T}"/>.
    /// </summary>
    public ref T Value => ref Get(0);

    /// <summary>
    /// Get the value at an index.
    /// </summary>
    /// <param name="index">The index to get the value at.</param>
    /// <returns>The value at <paramref name="index"/>.</returns>
    public ref T this[int index]
    {
        get => ref Get(index);
    }

    /// <summary>
    /// Get the value at an index.
    /// </summary>
    /// <param name="index">The index to get the value at.</param>
    /// <returns>The value at <paramref name="index"/>.</returns>
    public ref T this[long index]
    {
        get => ref Get(index);
    }

    /// <summary>
    /// Dispose of the <see cref="Reference{T}"/>.
    /// </summary>
    public void Dispose()
    {
        Managed<T>.Free(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ref T Get(int index = 0)
    {
        return ref Get((long) index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ref T Get(long index = 0)
    {
        return ref Unsafe.AsRef<T>(_pointer + index);
    }
}