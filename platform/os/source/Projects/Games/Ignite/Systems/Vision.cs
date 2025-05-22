// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Ignite.Contracts;
using Ignite.Models;
using Ignite.Models.Objects;
using Serilog;
using Spatial.Mathematics;
using Spatial.Simulation;
using Spatial.Structures;

namespace Ignite.Systems;

/// <summary>
/// A <see cref="System"/> that governs <see cref="ObjectRef"/> visibility.
/// </summary>
public class Vision : System
{
    private readonly Query _query;
    private readonly ConcurrentHashSet<Pair> _visibility;

    /// <summary>
    /// Create a new <see cref="Vision"/> <see cref="System"/>.
    /// </summary>
    /// <param name="map">The enclosing <see cref="Map"/>.</param>
    public Vision(Map map) : base(map)
    {
        _query = new Query().WithAll<Player>();
        _visibility = [];
    }

    /// <summary>
    /// Update a <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Map map, Time delta)
    {
        var visibility = (Current: new ConcurrentHashSet<Pair>(), Existing: new ConcurrentHashSet<Pair>(_visibility));

        map.Dynamic(_query, (Future future, in (Entity, Chunk) _, in Entity entity) => {
            var player = entity;
            var ta = map.Space.Get<Transform>(entity);

            var others = map.Grid.Query(
                position: ta,
                radius: map.Data.Info.Sight,
                filter: subject => subject != player && !map.Space.Has<Hidden>(subject) && map.TypeAt(subject) != ObjectType.Chunk);

            foreach (var other in others)
            {
                var tb = map.Space.Get<Transform>(other);
                var distance = Point2D.Distance(ta.X, ta.Y, tb.X, tb.Y);
                var range = map.Data.Info.Sight - (map.Space.Has<NPC>(other) ? Constants.NPC_VIEW_RANGE : 0);

                if (distance <= range)
                {
                    visibility.Current.Add(new Pair(other, player));
                }
            }
        });

        foreach (var pair in visibility.Current)
        {
            if (!visibility.Existing.Contains(pair) && map.Space.Exists(pair.First) && map.Space.Exists(pair.Second))
            {
                _visibility.Add(pair);

                var other = map.Ref(pair.First);
                var player = map.Ref(pair.Second);

                player.Focus(other);

                if (!player.Has<Hidden>() && other is PlayerRef)
                {
                    other.Focus(player);

                    Log.Debug("{Other} visible to {Player}.", player, other);
                }

                Log.Debug("{Other} visible to {Player}.", other, player);
            }
        }

        foreach (var pair in visibility.Existing)
        {
            if (!visibility.Current.Contains(pair))
            {
                _visibility.Remove(pair);

                if (map.Space.Exists(pair.First) && map.Space.Exists(pair.Second))
                {
                    var other = map.Ref(pair.First);
                    var player = map.Ref(pair.Second);

                    player.Blur(other);

                    if (other is PlayerRef)
                    {
                        other.Blur(player);

                        Log.Debug("{Other} no longer visible to {Player}.", player, other);
                    }

                    Log.Debug("{Other} no longer visible to {Player}.", other, player);
                }
            }
        }
    }
}