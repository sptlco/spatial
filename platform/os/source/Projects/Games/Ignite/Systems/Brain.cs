// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Ignite.Models;
using Spatial.Mathematics;
using Spatial.Simulation;

namespace Ignite.Systems;

/// <summary>
/// A <see cref="System"/> that controls intelligent objects.
/// </summary>
public class Brain : System
{
    private readonly Query _query;

    /// <summary>
    /// Create a new <see cref="Brain"/>.
    /// </summary>
    /// <param name="map">The enclosing <see cref="Map"/>.</param>
    public Brain(Map map) : base(map)
    {
        _query = new Query().WithAny<Intelligence, Script>().WithNone<Destination>();
    }

    /// <summary>
    /// Update a <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Map map, Time delta)
    {
        map.Dynamic(_query, (Future future, in (Entity, Chunk) _, in Entity entity) => {

            // Mobs with no destination have a 0.5% chance to walk to a random 
            // position in any direction.

            if (Strong.Boolean(0.005F))
            {
                var transform = map.Space.Get<Transform>(entity);
                var destination = Transform.Around(transform, 60.0F);

                map.ObjectAt(entity).Walk(destination, future);
            }
        });
    }
}