// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Simulation;

namespace Ignite.Systems;

public abstract class System : System<Space>
{
    private readonly Map _map;

    /// <summary>
    /// Create a new <see cref="System"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> the system belongs to.</param>
    public System(Map map)
    {
        _map = map;
    }

    /// <summary>
    /// Execute code before updating the <see cref="Space"/>.
    /// </summary>
    /// <param name="space">The <see cref="Space"/> to update.</param>
    public sealed override void BeforeUpdate(Space space)
    {
        BeforeUpdate(_map);
    }

    /// <summary>
    /// Update the <see cref="Space"/>.
    /// </summary>
    /// <param name="space">The <see cref="Space"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public sealed override void Update(Space space, Time delta)
    {
        Update(_map, delta);
    }

    /// <summary>
    /// Execute code after updating the <see cref="Space"/>.
    /// </summary>
    /// <param name="space">The <see cref="Space"/> to update.</param>
    public sealed override void AfterUpdate(Space space)
    {
        AfterUpdate(_map);
    }

    /// <summary>
    /// Execute code before updating the <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to update.</param>
    public virtual void BeforeUpdate(Map map) { }

    /// <summary>
    /// Update the <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public virtual void Update(Map map, Time delta) { }

    /// <summary>
    /// Execute code after updating the <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to update.</param>
    public virtual void AfterUpdate(Map map) { }
}
