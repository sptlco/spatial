// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Components;
using Spatial.Simulation;

namespace Ignite.Models.Objects;

/// <summary>
/// A reference to an NPC <see cref="Value"/>.
/// </summary>
public class NPCRef : ObjectRef
{
    /// <summary>
    /// Create a new <see cref="NPCRef"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> the NPC is in.</param>
    /// <param name="entity">The <see cref="Entity"/> to reference.</param>
    public NPCRef(Map map, Entity entity) : base(map, entity) { }

    /// <summary>
    /// The referenced <see cref="Components.NPC"/>.
    /// </summary>
    public NPC Value => Get<NPC>();

    /// <summary>
    /// The NPC's <see cref="Components.Gate"/>.
    /// </summary>
    public Gate? Gate => Has<Gate>() ? Get<Gate>() : null;

    /// <summary>
    /// Convert the <see cref="NPCRef"/> to a <see cref="string"/>.
    /// </summary>
    /// <returns>A <see cref="string"/>.</returns>
    public override string ToString()
    {
        return MobInfo.Load(Value.Id).Client.Name;
    }
}
