// Copyright © Spatial Corporation. All rights reserved.

using System.Runtime.CompilerServices;
using Spatial.Structures;

namespace Spatial.Simulation;

/// <summary>
/// The future state of a <see cref="Space"/>.
/// </summary>
public sealed partial class Future : IDisposable
{
    private static readonly InterlockedQueue<Future> _pool = new();

    private readonly InterlockedQueue<Action<Space>> _reserves;
    private readonly InterlockedQueue<Action<Space>> _creates;
    private readonly InterlockedQueue<Action<Space>> _sets;
    private readonly InterlockedQueue<Action<Space>> _adds;
    private readonly InterlockedQueue<Action<Space>> _removes;
    private readonly InterlockedQueue<Action<Space>> _destroys;
    private readonly InterlockedQueue<Action<Space>> _actions;

    private Future()
    {
        _reserves = new();
        _creates = new();
        _sets = new();
        _adds = new();
        _removes = new();
        _destroys = new();
        _actions = new();
    }

    /// <summary>
    /// Create a new <see cref="Future"/>.
    /// </summary>
    /// <returns>A <see cref="Future"/>.</returns>
    /// [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Future New()
    {
        if (!_pool.TryDequeue(out var future))
        {
            future = new();
        }

        return future;
    }

    /// <summary>
    /// Defer an action
    /// </summary>
    /// <param name="action">The action to defer.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Defer(Action<Space> action)
    {
        _actions.Enqueue(action);
    }

    /// <summary>
    /// Reserve space for entities.
    /// </summary>
    /// <param name="count">The number of entities to reserve space for.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reserve(uint count)
    {
        _reserves.Enqueue(space => space.Reserve(count));
    }

    /// <summary>
    /// Reserve space for entities.
    /// </summary>
    /// <param name="signature">The <see cref="Signature"/> to reserve space in.</param>
    /// <param name="count">The number of entities to reserve space for.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reserve(Signature signature, uint count)
    {
        _reserves.Enqueue(space => space.Reserve(signature, count));
    }

    /// <summary>
    /// Create a new <see cref="Entity"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Create()
    {
        _creates.Enqueue(space => space.Create());
    }

    /// <summary>
    /// Create a new <see cref="Entity"/>.
    /// </summary>
    /// <param name="signature">The entity's <see cref="Signature"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Create(Signature signature)
    {
        _creates.Enqueue(space => space.Create(signature));
    }

    /// <summary>
    /// Add a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to add.</typeparam>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<T>(Entity entity) where T : unmanaged, IComponent => Add(entity, new T());

    /// <summary>
    /// Add a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to add.</typeparam>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    /// <param name="component">The <see cref="IComponent"/> to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<T>(Entity entity, T component) where T : unmanaged, IComponent
    {
        _adds.Enqueue(space => space.Add(entity, component));
    }

    /// <summary>
    /// Remove a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to remove.</typeparam>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Remove<T>(Entity entity) where T : unmanaged, IComponent
    {
        _removes.Enqueue(space => space.Remove<T>(entity));
    }

    /// <summary>
    /// Set a <see cref="IComponent"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to set.</typeparam>
    /// <param name="entity">An <see cref="Entity"/> identification number.</param>
    /// <param name="component">The <see cref="IComponent"/> to set.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set<T>(Entity entity, T component) where T : unmanaged, IComponent
    {
        _sets.Enqueue(space => space.Set(entity, component));
    }

    /// <summary>
    /// Modify a component in place.
    /// </summary>
    /// <typeparam name="T">The type of component to modify.</typeparam>
    /// <param name="entity">The entity whose component to modify.</param>
    /// <param name="setter">A modification.</param>
    public void Modify<T>(Entity entity, Modification<T> setter) where T : unmanaged, IComponent
    {
        _sets.Enqueue(space => space.Modify(entity, setter));
    }

    /// <summary>
    /// Destroy an <see cref="Entity"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Destroy(Entity entity)
    {
        _destroys.Enqueue(space => space.Destroy(entity));
    }

    /// <summary>
    /// Commit the structural changes of the <see cref="Query"/>.
    /// </summary>
    /// <param name="space">The <see cref="Space"/> to submit the changes to.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Commit(Space space)
    {
        Playback(_reserves, space);
        Playback(_creates, space);
        Playback(_adds, space);
        Playback(_removes, space);
        Playback(_sets, space);
        Playback(_destroys, space);
        Playback(_actions, space);
    }

    /// <summary>
    /// Dispose of the <see cref="Future"/>.
    /// </summary>
    public void Dispose()
    {
        _reserves.Clear();
        _creates.Clear();
        _adds.Clear();
        _removes.Clear();
        _sets.Clear();
        _destroys.Clear();

        _pool.Enqueue(this);
    }

    private void Playback(InterlockedQueue<Action<Space>> queue, Space space)
    {
        while (queue.TryDequeue(out var change))
        {
            change(space);
        }
    }
}