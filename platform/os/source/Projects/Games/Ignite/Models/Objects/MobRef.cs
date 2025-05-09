// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Spatial.Simulation;

namespace Ignite.Models.Objects;

/// <summary>
/// A reference to a mob <see cref="Object"/>.
/// </summary>
public class MobRef : Object
{
    /// <summary>
    /// Create a new <see cref="MobRef"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> the mob is in.</param>
    /// <param name="entity">The <see cref="Entity"/> to reference.</param>
    public MobRef(Map map, Entity entity) : base(map, entity) { }

    /// <summary>
    /// The referenced <see cref="Mob"/>.
    /// </summary>
    public Mob Object => Get<Mob>();
}
