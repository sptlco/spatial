// Copyright © Spatial. All rights reserved.

using Ignite.Assets.Types;
using Ignite.Components;
using Ignite.Models;
using Ignite.Models.Objects;
using Serilog;
using Spatial.Mathematics;
using Spatial.Simulation;
using Spatial.Structures;

namespace Ignite.Systems;

/// <summary>
/// A <see cref="System"/> that governs <see cref="Object"/> visibility.
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
            var transform = map.Space.Get<Transform>(entity);

            var subjects = map.Grid.Query(
                position: transform,
                radius: map.Data.Info.Sight,
                filter: subject => subject != player && !map.Space.Has<Hidden>(subject) && map.TypeAt(subject) != ObjectType.Chunk);

            foreach (var subject in subjects)
            {
                var position = map.Space.Get<Transform>(subject);
                var distance = Point2D.Distance(transform.X, transform.Y, position.X, position.Y);

                if (distance <= map.Data.Info.Sight - Constants.U)
                {
                    visibility.Current.Add(new Pair(subject, player));
                }
            }
        });

        foreach (var pair in visibility.Current)
        {
            if (!visibility.Existing.Contains(pair) && map.Space.Exists(pair.First) && map.Space.Exists(pair.Second))
            {
                _visibility.Add(pair);

                var subject = map.ObjectAt(pair.First);
                var player = map.ObjectAt(pair.Second);

                player.Focus(subject);

                if (!player.Has<Hidden>() && subject is PlayerRef)
                {
                    subject.Focus(player);

                    Log.Debug("{Subject} visible to {Player}.", player, subject);
                }

                Log.Debug("{Subject} visible to {Player}.", subject, player);
            }
        }

        foreach (var pair in visibility.Existing)
        {
            if (!visibility.Current.Contains(pair))
            {
                _visibility.Remove(pair);

                if (map.Space.Exists(pair.First) && map.Space.Exists(pair.Second))
                {
                    var subject = map.ObjectAt(pair.First);
                    var player = map.ObjectAt(pair.Second);

                    player.Blur(subject);

                    if (subject is PlayerRef)
                    {
                        subject.Blur(player);

                        Log.Debug("{Subject} no longer visible to {Player}.", player, subject);
                    }

                    Log.Debug("{Subject} no longer visible to {Player}.", subject, player);
                }
            }
        }
    }
}