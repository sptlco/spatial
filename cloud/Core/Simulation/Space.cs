// Copyright © Spatial Corporation. All rights reserved.

using Serilog;
using Spatial.Compute.Jobs;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Spatial.Simulation;

/// <summary>
/// A container for all entities and their components.
/// </summary>
public sealed partial class Space : IDisposable
{
    private readonly List<System> _systems;

    private Entity[] _entities;
    private ConcurrentBag<uint> _entityPool;
    private uint _entityCount;

    private Archetype?[] _archetypes;
    private readonly ConcurrentDictionary<Signature, Archetype> _archetypeMap;
    private ConcurrentBag<uint> _archetypePool;
    private uint _archetypeCount;

    private Time _time;
    private int _disposed;
    private readonly Future _future;

    /// <summary>
    /// Create a new <see cref="Space"/>.
    /// </summary>
    internal Space(Time t0 = default)
    {
        _systems = [];

        _entities = [Entity.Null];
        _entityPool = [];
        _entityCount = 1;

        _archetypes = [new(this, 0, Signature.Empty)];
        _archetypeMap = new() { [Signature.Empty] = _archetypes[0]! };
        _archetypePool = [];
        _archetypeCount = 1;

        _time = t0;
        _future = Future.New();
    }

    /// <summary>
    /// The current <see cref="Time"/> in the <see cref="Space"/>.
    /// </summary>
    public Time Time => _time;

    /// <summary>
    /// Queued updates for the <see cref="Space"/>.
    /// </summary>
    public Future Future => _future;

    /// <summary>
    /// The entities in the <see cref="Space"/>.
    /// </summary>
    internal Entity[] Entities => _entities;

    /// <summary>
    /// The archetypes in the <see cref="Space"/>.
    /// </summary>
    internal Archetype?[] Archetypes => _archetypes;

    /// <summary>
    /// Create an empty <see cref="Space"/>.
    /// </summary>
    /// <returns>An empty <see cref="Space"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Space Empty()
    {
        return new Space();
    }

    /// <summary>
    /// Reserve space for entities.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reserve(uint count)
    {
        Reserve(Signature.Empty, count);
    }

    /// <summary>
    /// Reserve space for entities.
    /// </summary>
    /// <param name="signature">The <see cref="Signature"/> to reserve space in.</param>
    /// <param name="count">The number of entities to reserve space for.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reserve(in Signature signature, uint count)
    {
        var max = _entityCount + count;

        if (max > _entities.Length)
        {
            Array.Resize(ref _entities, (int) Math.Max(_entities.Length * 2, max));
        }

        GetOrCreateArchetype(signature).Reserve(count);
    }

    /// <summary>
    /// Create a new <see cref="Entity"/>.
    /// </summary>
    /// <returns>A new <see cref="Entity"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref Entity Create()
    {
        return ref Create(Signature.Empty);
    }

    /// <summary>
    /// Create a new <see cref="Entity"/>.
    /// </summary>
    /// <param name="signature">The entity's <see cref="Signature"/>.</param>
    /// <returns>A new <see cref="Entity"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref Entity Create(in Signature signature)
    {
        ref var entity = ref CreateId();

        GetOrCreateArchetype(signature).Add(ref entity);

        return ref entity;
    }

    /// <summary>
    /// Create a <see cref="Simulation.Group"/> of entities.
    /// </summary>
    /// <param name="entities">A collection of entities.</param>
    /// <returns>A <see cref="Simulation.Group"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity Group(IEnumerable<Entity> entities)
    {
        var parent = Create();
        var group = new Group(parent);

        foreach (var child in entities)
        {
            Add(child, group);
        }

        return parent;
    }

    /// <summary>
    /// Add an <see cref="Entity"/> to a <see cref="Group"/>.
    /// </summary>
    /// <param name="child">An <see cref="Entity"/>.</param>
    /// <param name="group">A <see cref="Simulation.Group"/>.</param>
    public void Join(in Entity child, in Entity group)
    {
        Add(child, new Group(group));
    }

    /// <summary>
    /// Get the parent <see cref="Entity"/> of an <see cref="Entity"/>.
    /// </summary>
    /// <param name="child">An <see cref="Entity"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity GetParent(in Entity child)
    {
        return Get<Group>(child).Id;
    }

    /// <summary>
    /// Get the parent <see cref="Entity"/> of an <see cref="Entity"/>.
    /// </summary>
    /// <typeparam name="T">The type of parent to get.</typeparam>
    /// <param name="child">An <see cref="Entity"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Entity GetParent<T>(in Entity child) where T : unmanaged, IComponent
    {
        var parent = GetParent(child);

        while (!Has<T>(parent))
        {
            parent = GetParent(parent);
        }

        return parent;
    }

    /// <summary>
    /// Remove an <see cref="Entity"/> from a <see cref="Simulation.Group"/>.
    /// </summary>
    /// <param name="child">An <see cref="Entity"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Exile(in Entity child)
    {
        Remove<Group>(child);
    }

    /// <summary>
    /// Remove all entities from a <see cref="Simulation.Group"/>.
    /// </summary>
    /// <param name="group">A <see cref="Simulation.Group"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(in Entity group)
    {
        foreach (var child in Query(group))
        {
            Remove<Group>(child);
        }
    }

    /// <summary>
    /// Add a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to add.</typeparam>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<T>(in Entity entity) where T : unmanaged, IComponent => Add(entity, new T());

    /// <summary>
    /// Add a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to add.</typeparam>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    /// <param name="component">The <see cref="IComponent"/> to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<T>(in Entity entity, in T component) where T : unmanaged, IComponent
    {
        Log.Verbose("Adding {component} to {entity}", typeof(T).Name, entity);

        ref var handle = ref _entities[entity];

        var archetype = _archetypes[handle.Archetype]!;

        if (!archetype.Signature.Includes<T>())
        {
            var signature = archetype.Signature | Signature.Of<T>();
        
            CopyToArchetype(
                entity: ref handle, 
                source: archetype,
                destination: GetOrCreateArchetype(signature));
        }

        Set(entity, component);
    }

    /// <summary>
    /// Remove a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to remove.</typeparam>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove<T>(in Entity entity) where T : unmanaged, IComponent
    {
        Log.Verbose("Removing {component} from {entity}", typeof(T).Name, entity);

        ref var handle = ref _entities[entity];

        var archetype = _archetypes[handle.Archetype]!;
        
        if (archetype.Signature.Includes<T>())
        {
            var signature = archetype.Signature & (~Signature.Of<T>());

            CopyToArchetype(
                entity: ref handle,
                source: archetype,
                destination: GetOrCreateArchetype(signature));
        }
    }

    /// <summary>
    /// Destroy all entities with a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to remove.</typeparam>
    /// <param name="filter">A filter for the removal.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove<T>(Func<T, bool> filter) where T : unmanaged, IComponent
    {
        Mutate(
            query: new Query().WithAll<T>(),
            filter: (entity) => filter(Get<T>(entity)),
            function: (Future future, in Entity entity) => {
                future.Remove<T>(entity);
            });
    }

    /// <summary>
    /// Determine whether or not an <see cref="Entity"/> has a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">A <see cref="IComponent"/> type.</typeparam>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    /// <returns>Whether or not the <see cref="Entity"/> has the <see cref="IComponent"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has<T>(in Entity entity) where T : unmanaged, IComponent
    {
        ref var handle = ref _entities[entity];

        return _archetypes[handle.Archetype]!.Signature.Includes<T>();
    }

    /// <summary>
    /// Get whether or not a component exists.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to query.</typeparam>
    /// <param name="query">A filter for the space.</param>
    /// <returns>Whether or not the component exists.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Exists<T>(Func<T, bool>? query = default) where T : unmanaged, IComponent
    {
        foreach (var (_, archetype) in _archetypeMap)
        {
            if (archetype.Signature.Includes<T>())
            {
                for (var m = 0; m < archetype.Chunks.Length; m++)
                {
                    var chunk = archetype.Chunks[m];

                    for (uint n = 0; n < chunk.Count; n++)
                    {
                        ref var component = ref chunk.Ref<T>(n);

                        if (query?.Invoke(component) ?? true)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Get a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to get.</typeparam>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    /// <returns>A <see cref="IComponent"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T Get<T>(in Entity entity) where T : unmanaged, IComponent
    {
        ref var handle = ref _entities[entity];
        
        var archetype = _archetypes[handle.Archetype]!;

        if (!archetype.Signature.Includes<T>())
        {
            throw new InvalidOperationException("The component does not exist.");
        }
        
        return ref archetype.Chunks[handle.Chunk].Ref<T>(handle.Index);
    }

    /// <summary>
    /// Set a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to set.</typeparam>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    /// <param name="component"><see cref="IComponent"/> data.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set<T>(in Entity entity, in T component) where T : unmanaged, IComponent
    {
        ref var handle = ref _entities[entity];
        
        var archetype = _archetypes[handle.Archetype]!;

        if (!archetype.Signature.Includes<T>())
        {
            throw new InvalidOperationException("The component does not exist.");
        }

        archetype.Chunks[handle.Chunk].Set(handle.Index, component);

        Log.Verbose("Set {component} {entity} to {value}", typeof(T).Name, entity, component);
    }

    /// <summary>
    /// Modify a component in place.
    /// </summary>
    /// <typeparam name="T">The type of component to modify.</typeparam>
    /// <param name="entity">The entity whose component to modify.</param>
    /// <param name="setter">A modification.</param>
    public void Modify<T>(in Entity entity, Modification<T> setter) where T : unmanaged, IComponent
    {
        ref var handle = ref _entities[entity];

        var archetype = _archetypes[handle.Archetype]!;

        if (!archetype.Signature.Includes<T>())
        {
            throw new InvalidOperationException("The component does not exist.");
        }

        setter(ref archetype.Chunks[handle.Chunk].Ref<T>(handle.Index));
    }

    /// <summary>
    /// Count components in the <see cref="Space"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to query.</typeparam>
    /// <param name="query">A filter for the space.</param>
    /// <returns>A list of components of type <typeparamref name="T"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count<T>(Func<T, bool>? query = default) where T : unmanaged, IComponent
    {
        var count = 0;

        foreach (var (_, archetype) in _archetypeMap)
        {
            if (archetype.Signature.Includes<T>())
            {
                for (var m = 0; m < archetype.Chunks.Length; m++)
                {
                    var chunk = archetype.Chunks[m];

                    for (uint n = 0; n < chunk.Count; n++)
                    {
                        ref var entity = ref chunk.Entities[n];
                        ref var handle = ref _entities[entity];
                        ref var component = ref chunk.Ref<T>(handle.Index);

                        if (query == default || query(component))
                        {
                            count += 1;
                        }
                    }
                }
            }
        }

        return count;
    }

    /// <summary>
    /// Get or create a single instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to get.</typeparam>
    /// <returns>A singleton instance of the specified type.</returns>
    public ref T Singleton<T>() where T : unmanaged, IComponent
    {
        var entities = Query<T>(_ => true);
        var entity = entities.Any() ? entities.First() : Create<T>();

        return ref _archetypes[_entities[entity].Archetype]!.Chunks[0].Ref<T>(0);
    }

    /// <summary>
    /// Query the <see cref="Space"/>.
    /// </summary>
    /// <param name="query">A filter for the space.</param>
    /// <returns>A list of entities.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<Entity> Query(Query query)
    {
        foreach (var (_, archetype) in _archetypeMap)
        {
            if (query.Matches(archetype.Signature))
            {
                for (var m = 0; m < archetype.Chunks.Length; m++)
                {
                    var chunk = archetype.Chunks[m];

                    for (uint n = 0; n < chunk.Count; n++)
                    {
                        ref var entity = ref chunk.Entities[n];
                        ref var handle = ref _entities[entity];
                        
                        yield return handle;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Query the <see cref="Space"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to query.</typeparam>
    /// <param name="query">A filter for the space.</param>
    /// <returns>A list of entities satisfying the query.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<Entity> Query<T>(Func<T, bool>? query = default) where T : unmanaged, IComponent
    {
        foreach (var (_, archetype) in _archetypeMap)
        {
            if (archetype.Signature.Includes<T>())
            {
                for (var m = 0; m < archetype.Chunks.Length; m++)
                {
                    var chunk = archetype.Chunks[m];

                    for (uint n = 0; n < chunk.Count; n++)
                    {
                        ref var entity = ref chunk.Entities[n];
                        ref var handle = ref _entities[entity];
                        ref var component = ref chunk.Ref<T>(handle.Index);

                        if (query == default || query(component))
                        {
                            yield return handle;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Query the <see cref="Space"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to query.</typeparam>
    /// <param name="query">A filter for the space.</param>
    /// <returns>A list of entities satisfying the query.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<Entity> Query<T>(Func<Entity, T, bool>? query = default) where T : unmanaged, IComponent
    {
        foreach (var (_, archetype) in _archetypeMap)
        {
            if (archetype.Signature.Includes<T>())
            {
                for (var m = 0; m < archetype.Chunks.Length; m++)
                {
                    var chunk = archetype.Chunks[m];

                    for (uint n = 0; n < chunk.Count; n++)
                    {
                        ref var entity = ref chunk.Entities[n];
                        ref var handle = ref _entities[entity];
                        ref var component = ref chunk.Ref<T>(handle.Index);

                        if (query == default || query(entity, component))
                        {
                            yield return handle;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Get a <see cref="Simulation.Group"/> of entities.
    /// </summary>
    /// <param name="group">The <see cref="Simulation.Group"/> to query.</param>
    /// <returns>A <see cref="Simulation.Group"/> of entities.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<Entity> Query(Entity group)
    {
        return Query<Group>(g => g.Id == group);
    }

    /// <summary>
    /// Mutate the <see cref="Space"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Mutate(Query query, MutateFunction function, Func<Entity, bool>? filter = default)
    {
        void process(Archetype archetype, int m, int n)
        {
            var chunk = archetype.Chunks[m];

            if (n < chunk.Count)
            {
                ref var entity = ref chunk.Entities[n];
                ref var handle = ref _entities[entity];

                if (filter?.Invoke(handle) ?? true)
                {
                    function(_future, handle);
                }
            }
        }

        foreach (var (_, archetype) in _archetypeMap)
        {
            if (!query.Matches(archetype.Signature))
            {
                continue;
            }

            if (query.Accelerated)
            {
                Job.ParallelFor2D(
                    width: archetype.Chunks.Length,
                    height: (int) archetype.Chunks.Max(c => c.Count),
                    function: (m, n) => process(archetype, m, n));
            }
            else
            {
                for (var m = 0; m < archetype.Chunks.Length; m++)
                {
                    for (var n = 0; n < archetype.Chunks[m].Count; n++)
                    {
                        process(archetype, m, n);
                    }
                }
            }
        }

        _future.Commit(this);
    }

    /// <summary>
    /// Get whether or not an <see cref="Entity"/> exists.
    /// </summary>
    /// <param name="entity">An <see cref="Entity"/>.</param>
    /// <returns>Whether or not the <see cref="Entity"/> exists.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Exists(in Entity entity)
    {
        return entity != Entity.Null && _entities[entity] != Entity.Null;
    }

    /// <summary>
    /// Destroy an <see cref="Entity"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Destroy(in Entity entity)
    {
        ref var handle = ref _entities[entity];

        RemoveFromArchetype(handle);

        _entities[entity] = Entity.Null;
        _entityPool.Add(entity);
    }

    /// <summary>
    /// Dispose of the <see cref="Space"/>.
    /// </summary>
    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        _systems.ForEach(sys => sys.Destroy(this));

        for (var i = 0; i < _archetypes.Length; i++)
        {
            _archetypes[i]?.Dispose();
        }

        _systems.Clear();

        _entityPool.Clear();
        _archetypePool.Clear();

        _entities = null!;
        _archetypes = null!;
        _entityPool = null!;
        _archetypePool = null!;
    }

    private ref Entity CreateId()
    {
        if (!_entityPool.TryTake(out var id))
        {
            if ((id = _entityCount++) >= _entities.Length)
            {
                Array.Resize(ref _entities, Math.Max(_entities.Length * 2, (int) id + 1));
            }
        }

        ref var entity = ref _entities[id];
        entity = new(id);
        return ref entity;
    }

    private Archetype GetOrCreateArchetype(in Signature signature)
    {
        // Note: ConcurrentDictionary<>.GetOrAdd allocates memory.
        // Benchmarks show a 64B allocation, so let's use TryGetValue.

        if (_archetypeMap.TryGetValue(signature, out var archetype))
        {
            return archetype;
        }

        if (!_archetypePool.TryTake(out var archetypeId))
        {
            if ((archetypeId = _archetypeCount++) >= _archetypes.Length)
            {
                Array.Resize(ref _archetypes, _archetypes.Length * 2);
            }
        }

        return _archetypeMap[signature] = _archetypes[archetypeId] = new(this, archetypeId, signature);
    }

    private unsafe void CopyToArchetype(ref Entity entity, Archetype source, Archetype destination)
    {        
        Log.Verbose("Copying {entity} from {archetype} ({source}, {chunk}, {index}) to {destination}.", entity.Id, source.Id, entity.Archetype, entity.Chunk, entity.Index, destination.Id);

        var copy = entity;

        destination.Add(ref entity);

        var sourceChunk = source.Chunks[copy.Chunk];
        var destinationChunk = destination.Chunks[entity.Chunk];

        for (var i = 0; i < source.Signature.Components.Count; i++)
        {
            var component = source.Signature.Components[i];

            if (destination.Signature.Includes(component))
            {
                var componentId = ComponentRegistry.GetComponentId(component);
                var componentSize = Marshal.SizeOf(component);

                Managed.Copy(
                    source: sourceChunk.Components,
                    sourceOffset: (copy.Index * source.Stride) + source.Offsets[componentId],
                    destination: destinationChunk.Components,
                    destinationOffset: (entity.Index * destination.Stride) + destination.Offsets[componentId],
                    size: componentSize
                );
            }
        }

        RemoveFromArchetype(copy);
    }

    private void RemoveFromArchetype(in Entity entity)
    {
        ref var arch = ref _archetypes[entity.Archetype]!;

        var archetype = arch.Id;
        var signature = arch.Signature;

        arch.Remove(entity);

        if (archetype > 0 && arch.Count == 0)
        {
            Log.Verbose("Disposing of archetype {archetype}", arch.Id);

            arch.Dispose();
            arch = null;

            _archetypeMap.TryRemove(signature, out _);
            _archetypePool.Add(archetype);
        }
    }
}

// The following methods are lifecycle methods.
// There are also methods that allow you to hook into these lifecycle
// methods by implementing systems.

public sealed partial class Space
{
    /// <summary>
    /// Initialize the <see cref="Space"/>.
    /// </summary>
    public void Initialize()
    {
        _systems.ForEach(sys => sys.Create(this));
    }

    /// <summary>
    /// Update the <see cref="Space"/>.
    /// </summary>
    public void Update(Time delta)
    {
        _systems.ForEach(sys => sys.BeforeUpdate(this));
        _systems.ForEach(sys => sys.Update(this, delta));
        _systems.ForEach(sys => sys.AfterUpdate(this));

        _time += delta;
    }

    /// <summary>
    /// Use a <see cref="System"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="System"/> to run.</typeparam>
    /// <returns>The <see cref="Space"/> for method chaining.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Space Use<T>(params object[] args) where T : System
    {
        return Use(_ => (T) Activator.CreateInstance(typeof(T), args)!);
    }

    /// <summary>
    /// Use a <see cref="System"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="System"/> to run.</typeparam>
    /// <param name="system">The <see cref="System"/> to use.</param>
    /// <returns>The <see cref="Space"/> for method chaining.</returns>
    public Space Use<T>(T system) where T : System
    {
        return Use(_ => system);
    }

    /// <summary>
    /// Use a <see cref="System"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="System"/> to run.</typeparam>
    /// <param name="builder">A <see cref="System"/> builder.</param>
    /// <returns>The <see cref="Space"/> for method chaining.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Space Use<T>(Func<Space, T> builder) where T : System
    {
        _systems.Add(builder(this));
        return this;
    }
}

/// <summary>
/// Set a component in place.
/// </summary>
/// <typeparam name="T">The type of component to set.</typeparam>
/// <param name="component">A reference to the component.</param>
public delegate void Modification<T>(ref T component) where T : unmanaged, IComponent;