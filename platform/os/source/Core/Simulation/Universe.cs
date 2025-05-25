// Copyright © Spatial. All rights reserved.

using System.Runtime.CompilerServices;
using Spatial.Compute.Jobs;

namespace Spatial.Simulation;

/// <summary>
/// A collection of <see cref="Space"/>.
/// </summary>
public sealed class Universe : IDisposable
{
    private readonly List<Space> _spaces;
    private readonly List<System<Universe>> _systems;
    private readonly Lock _lock;

    private Universe()
    {
        _spaces = [];
        _systems = [];
        _lock = new();
    }

    /// <summary>
    /// Create a new <see cref="Universe"/>.
    /// </summary>
    /// <returns>A <see cref="Universe"/>.</returns>
    public static Universe Create()
    {
        return new();
    }

    /// <summary>
    /// Add a <see cref="Space"/> to the <see cref="Universe"/>.
    /// </summary>
    /// <param name="space">A <see cref="Space"/>.</param>
    public void Add(Space space)
    {
        lock (_lock)
        {
            _spaces.Add(space);
        }
    }

    /// <summary>
    /// Remove a <see cref="Space"/> from the <see cref="Universe"/>.
    /// </summary>
    /// <param name="space">A <see cref="Space"/>.</param>
    public void Remove(Space space)
    {
        lock (_lock)
        {
            _spaces.Remove(space);
        }
    }

    /// <summary>
    /// Update the <see cref="Universe"/>.
    /// </summary>
    /// <param name="delta"><see cref="Time"/> passed since the last update.</param>
    public void Update(Time delta)
    {
        _systems.ForEach(system => system.BeforeUpdate(this));
        _systems.ForEach(system => system.Update(this, delta));
        _systems.ForEach(system => system.AfterUpdate(this));

        lock (_lock)
        {
            Job.ParallelFor(_spaces, space => space.Update(delta));
        }
    }

    /// <summary>
    /// Use a <see cref="System{Universe}"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="System{Universe}"/> to run.</typeparam>
    /// <returns>The <see cref="Universe"/> for method chaining.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Universe Use<T>() where T : System<Universe>, new()
    {
        return Use(_ => new T());
    }

    /// <summary>
    /// Use a <see cref="System{Universe}"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="System{Universe}"/> to run.</typeparam>
    /// <param name="system">The <see cref="System{Universe}"/> to use.</param>
    /// <returns>The <see cref="Universe"/> for method chaining.</returns>
    public Universe Use<T>(T system) where T : System<Universe>
    {
        return Use(_ => system);
    }

    /// <summary>
    /// Use a <see cref="System{Universe}"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="System{Universe}"/> to run.</typeparam>
    /// <param name="builder">A <see cref="System{Universe}"/> builder.</param>
    /// <returns>The <see cref="Universe"/> for method chaining.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Universe Use<T>(Func<Universe, T> builder) where T : System<Universe>
    {
        _systems.Add(builder(this));
        return this;
    }

    /// <summary>
    /// Dispose of the <see cref="Universe"/>.
    /// </summary>
    public void Dispose()
    {
        lock (_lock)
        {
            foreach (var space in _spaces)
            {
                space.Dispose();
            }

            _spaces.Clear();
        }
    }
}