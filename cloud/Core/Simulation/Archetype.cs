// Copyright © Spatial Corporation. All rights reserved.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Serilog;

namespace Spatial.Simulation;

/// <summary>
/// A group of entities with an identical set of components.
/// </summary>
internal sealed unsafe class Archetype : IDisposable
{
    private Space _space;
    private readonly uint _id;
    private readonly Signature _signature;
    private readonly int _stride;
    private readonly Dictionary<int, int> _offsets;
    private readonly int _capacity;
    private Chunk[] _chunks;
    private uint _ptr;
    
    /// <summary>
    /// Create a new <see cref="Archetype"/>.
    /// </summary>
    /// <param name="registry">The <see cref="Space"/> that the <see cref="Archetype"/> belongs to.</param>
    /// <param name="id">The archetype's identification number.</param>
    /// <param name="signature">The <see cref="Signature"/> of the <see cref="Archetype"/>.</param>
    public Archetype(Space registry, in uint id, Signature signature)
    {
        _space = registry;
        _id = id;
        _signature = signature;
        _stride = _signature.Components.Sum(Marshal.SizeOf);
        _offsets = CalculateOffsets();
        _capacity = Math.Max(1, Constants.ChunkSize / (sizeof(uint) + _stride));
        _chunks = [new(this, 0)];
    }

    /// <summary>
    /// The <see cref="Space"/> that the <see cref="Archetype"/> belongs to.
    /// </summary>
    public Space Space => _space;

    /// <summary>
    /// The archetype's identification number.
    /// </summary>
    public uint Id => _id;

    /// <summary>
    /// The <see cref="Signature"/> of the <see cref="Archetype"/>.
    /// </summary>
    public Signature Signature => _signature;

    /// <summary>
    /// The size of each component segment in the <see cref="Archetype"/>.
    /// </summary>
    public int Stride => _stride;

    /// <summary>
    /// The offset of each component in the <see cref="Archetype"/>.
    /// </summary>
    public Dictionary<int, int> Offsets => _offsets;

    /// <summary>
    /// The number of entities in the <see cref="Archetype"/>.
    /// </summary>
    public uint Count => _ptr;

    /// <summary>
    /// The capacity of a <see cref="Chunk"/> in the <see cref="Archetype"/>.
    /// </summary>
    public int Capacity => _capacity;

    /// <summary>
    /// Storage units for entities in the <see cref="Archetype"/>.
    /// </summary>
    public Chunk[] Chunks => _chunks;

    /// <summary>
    /// Reserve space for entities.
    /// </summary>
    /// <param name="count">The number of entities to reserve space for.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reserve(uint count)
    {
        Resize(_chunks.Length + (int) ((count + _capacity - 1) / _capacity));
    }

    /// <summary>
    /// Add an <see cref="Entity"/> to the <see cref="Archetype"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(ref Entity entity)
    {
        entity.Archetype = _id;
        entity.Chunk = (uint) (_ptr / _capacity);
        entity.Index = (uint) (_ptr % _capacity);

        _ptr += 1;

        Log.Verbose("Adding {entity} to {archetype} ({chunk}, {index})", entity.Id, _id, entity.Chunk, entity.Index);

        if (entity.Chunk >= _chunks.Length)
        {
            Resize((int) Math.Max(_chunks.Length * 2, entity.Chunk + 1));
        }

        _chunks[entity.Chunk].Emplace(entity.Id);
    }

    /// <summary>
    /// Remove an <see cref="Entity"/> from the <see cref="Archetype"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> to remove.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove(in Entity entity)
    {
        _ptr -= 1;

        var chunkId = (uint) (_ptr / _capacity);
        var chunkIndex = (uint) (_ptr % _capacity);

        var source = _chunks[chunkId];
        var destination = _chunks[entity.Chunk];

        if (chunkId != entity.Chunk || chunkIndex != entity.Index)
        {
            destination.Entities[entity.Index] = source.Entities[chunkIndex];
        
            Managed.Copy(
                source: source.Components,
                sourceOffset: chunkIndex * _stride,
                destination: destination.Components,
                destinationOffset: entity.Index * _stride,
                size: _stride);

            ref var ent = ref _space.Entities[destination.Entities[entity.Index]];

            ent.Chunk = entity.Chunk;
            ent.Index = entity.Index;
        }

        source.Trim();

        var required = (int) Math.Max(1, (_ptr + _capacity - 1) / _capacity);

        if (required < _chunks.Length)
        {
            Resize(required);
        }
    }

    /// <summary>
    /// Dispose of the <see cref="Archetype"/>.
    /// </summary>
    public void Dispose()
    {
        for (var i = 0; i < _chunks.Length; i++)
        {
            _chunks[i].Dispose();
        }

        _space = null!;
        _chunks = null!;
    }

    private Dictionary<int, int> CalculateOffsets()
    {
        var components = _signature.Components.OrderBy(ComponentRegistry.GetComponentId).ToArray();

        var offsets = new Dictionary<int, int>();
        var offset = 0;

        for (var i = 0; i < components.Length; i++)
        {
            var component = components[i];

            offsets[ComponentRegistry.GetComponentId(component)] = offset;
            offset += Marshal.SizeOf(component);
        }

        return offsets;
    }

    private void Resize(int size)
    {
        var s0 = _chunks.Length;

        if (size < s0)
        {
            for (var i = size; i < s0; i++)
            {
                _chunks[i].Dispose();
            }
        }

        Array.Resize(ref _chunks, size);

        for (var i = s0; i < _chunks.Length; i++)
        {
            _chunks[i] = new(this, (uint) i);
        }
    }
}
