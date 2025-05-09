// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Simulation;

namespace Ignite.Systems;

/// <summary>
/// A <see cref="System"/> that debugs a <see cref="Map"/>.
/// </summary>
public class Debugger : System
{
    private readonly Query _query;

    /// <summary>
    /// Create a new <see cref="Debugger"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to debug.</param>
    public Debugger(Map map) : base(map)
    {
        _query = new Query();
    }

    /// <summary>
    /// Update the <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> passed since the last update.</param>
    public override void Update(Map map, Time delta)
    {
        // ...
    }
}