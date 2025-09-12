// Copyright © Spatial Corporation. All rights reserved.

using System.Runtime.InteropServices;

namespace Spatial;

/// <summary>
/// A contiguous block of unmanaged memory.
/// </summary>
public unsafe class Managed : Managed<byte>
{
}

/// <summary>
/// A contiguous block of unmanaged memory.
/// </summary>
/// <typeparam name="T">The type of elements in the <see cref="Managed{T}"/>.</typeparam>
public unsafe class Managed<T> where T : unmanaged
{
    /// <summary>
    /// Allocate <see cref="Managed{T}"/>.
    /// </summary>
    /// <param name="size">The size of the <see cref="Managed{T}"/> to allocate.</param>
    /// <returns>A reference to the allocated <see cref="Managed{T}"/>.</returns>
    public static Reference<T> Allocate(int size = 1)
    {
        return new Reference<T>((T*) NativeMemory.AllocZeroed((nuint) (size * sizeof(T))));
    }

    /// <summary>
    /// Free <see cref="Managed{T}"/>.
    /// </summary>
    /// <param name="reference">A reference to the <see cref="Managed{T}"/> to free.</param>
    public static void Free(Reference<T> reference)
    {
        NativeMemory.Free(reference.Pointer);
    }

    /// <summary>
    /// Copy a block of memory to another location.
    /// </summary>
    /// <param name="source">The block of memory to copy.</param>
    /// <param name="sourceOffset">An offset position for <paramref name="source"/>.</param>
    /// <param name="destination">The block of memory to copy to.</param>
    /// <param name="destinationOffset">An offset position within <paramref name="destination"/>.</param>
    /// <param name="size">The number of elements to copy.</param>
    public static void Copy(
        Reference<T> source, 
        long sourceOffset, 
        Reference<T> destination, 
        long destinationOffset, 
        long size)
    {
        NativeMemory.Copy(source.Pointer + sourceOffset, destination.Pointer + destinationOffset, (nuint) size);
    }
}