// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Components;
using Spatial.Simulation;

namespace Ignite.Models.Objects;

/// <summary>
/// A reference to an NPC <see cref="NPC"/>.
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
    public NPC NPC => Get<NPC>();

    /// <summary>
    /// The NPC's <see cref="Components.Gate"/>.
    /// </summary>
    public Gate? Gate => Has<Gate>() ? Get<Gate>() : null;

    /// <summary>
    /// Play the NPC's <see cref="NPCRole"/>.
    /// </summary>
    /// <param name="player">The <see cref="PlayerRef"/> interacting with the <see cref="NPCRef"/>.</param>
    public void Play(PlayerRef player)
    {
        switch (NPC.Role)
        {
            case NPCRole.Gate when Gate is Gate gate:
                var field = Field.Find(gate.Map);
                var map = Asset.First<MapInfo>("MapInfo.shn", m => m.MapName == field.MapIDClient);

                player.Prompt(
                    priority: 0,
                    title: Assets.Types.Script.String("MenuString", "LinkTitle", map.Name),
                    range: 1000F,
                    sender: this,
                    items: [
                        new(Assets.Types.Script.String("ETC", "Yes"), () => player.Teleport(gate.Map, gate.Id, new Transform(gate.X, gate.Y, gate.R))),
                        new(Assets.Types.Script.String("ETC", "No"), () => { }),
                    ]);

                break;
        }
    }

    /// <summary>
    /// Convert the <see cref="NPCRef"/> to a <see cref="string"/>.
    /// </summary>
    /// <returns>A <see cref="string"/>.</returns>
    public override string ToString()
    {
        return MobInfo.Load(NPC.Id).Client.Name;
    }
}
