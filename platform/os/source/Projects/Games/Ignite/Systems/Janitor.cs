// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Ignite.Models;
using Ignite.Models.Objects;
using Spatial.Extensions;
using Spatial.Simulation;

namespace Ignite.Systems;

/// <summary>
/// A <see cref="System"/> that periodically saves player data to the database.
/// </summary>
public class Janitor : System
{
    private readonly Query _query;

    /// <summary>
    /// Create a new <see cref="Progress"/> <see cref="System"/>.
    /// </summary>
    /// <param name="map">A <see cref="Map"/>.</param>
    public Janitor(Map map) : base(map)
    {
        _query = new Query().WithAll<Player, Dirty>();
    }

    /// <summary>
    /// Update a <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> passed since the last update.</param>
    public override void Update(Map map, Time delta)
    {
        map.Dynamic(_query, (Future future, in (Entity, Chunk) _, in Entity entity) => {
            ref var player = ref map.Space.Get<Player>(entity);

            if (World.Time - player.Saved >= Time.FromSeconds(6))
            {
                player.Saved = World.Time;

                future.Remove<Dirty>(entity);
                map.Ref<PlayerRef>(entity).Character.Save();
            }
        });
    }
}
