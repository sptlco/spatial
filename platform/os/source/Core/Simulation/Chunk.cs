// Copyright © Spatial. All rights reserved.

using Spatial.Hardware;
using System.Runtime.CompilerServices;

namespace Spatial.Simulation;

/// <summary>
/// A contiguous block of memory that stores component data.
/// </summary>
internal unsafe sealed partial class Chunk : IDisposable
{
    private readonly uint _id;
    private Archetype _archetype;
    private Reference<uint> _entities;
    private Reference<byte> _components;
    private uint _ptr;

    /// <summary>
    /// Create a new <see cref="Chunk"/>.
    /// </summary>
    public Chunk(Archetype archetype, uint id)
    {
        _id = id;
        _archetype = archetype;
        _entities = Managed<uint>.Allocate(_archetype.Capacity);
        _components = Managed.Allocate(_archetype.Stride * _archetype.Capacity);
    }

    /// <summary>
    /// The chunk's <see cref="Archetype"/>.
    /// </summary>
    public Archetype Archetype => _archetype;

    /// <summary>
    /// The chunk's identification number.
    /// </summary>
    public uint Id => _id;

    /// <summary>
    /// The chunk's entity data.
    /// </summary>
    public Reference<uint> Entities => _entities;

    /// <summary>
    /// The chunk's component data.
    /// </summary>
    public Reference<byte> Components => _components;

    /// <summary>
    /// The number of entities in the <see cref="Chunk"/>.
    /// </summary>
    public uint Count => _ptr;

    /// <summary>
    /// Add an <see cref="Entity"/> to the <see cref="Chunk"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Emplace(in uint entity)
    {
        _entities[_ptr++] = entity;
    }

    /// <summary>
    /// Trim the <see cref="Chunk"/>.
    /// </summary>
    /// <returns>The index of the last <see cref="Entity"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Trim()
    {
        _entities[--_ptr] = Entity.Null;
    }

    /// <summary>
    /// Get a reference to data in the <see cref="Chunk"/>.
    /// </summary>
    /// <typeparam name="T">The type of data to get.</typeparam>
    /// <param name="index">An <see cref="Entity"/> identification number.</param>
    /// <returns>A reference to data in the <see cref="Chunk"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T Ref<T>(uint index) where T : unmanaged, IComponent
    {
        return ref Unsafe.AsRef<T>(_components.Pointer + (index * _archetype.Stride) + _archetype.Offsets[Component<T>.Id]);
    }

    /// <summary>
    /// Set data in the <see cref="Chunk"/>.
    /// </summary>
    /// <typeparam name="T">The type of data to set.</typeparam>
    /// <param name="index">An <see cref="Entity"/> identification number.</param>
    /// <param name="value">A value of type <typeparamref name="T"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set<T>(uint index, in T value) where T : unmanaged, IComponent
    {
        Unsafe.WriteUnaligned(_components.Pointer + (index * _archetype.Stride) + _archetype.Offsets[Component<T>.Id], value);
    }

    /// <summary>
    /// Dispose of the <see cref="Chunk"/>.
    /// </summary>
    public void Dispose()
    {
        _entities.Dispose();
        _components.Dispose();

        _archetype = null!;
        _entities = null!;
        _components = null!;
    }
}