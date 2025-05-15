// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Ignite.Models;
using Serilog;
using Spatial.Mathematics;
using Spatial.Simulation;
using Spatial.Structures;

namespace Ignite.Systems;

/// <summary>
/// A <see cref="System"/> that computes the state of a <see cref="Map"/>.
/// </summary>
public class Computer : System
{
    private readonly Query _q1;
    private readonly Query _q2;

    private readonly ConcurrentHashSet<Pair> _collisions;

    /// <summary>
    /// Create a new <see cref="Computer"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> controlled by the <see cref="System"/>.</param>
    public Computer(Map map) : base(map)
    {
        _q1 = new Query().WithAll<Tag, Transform>();
        _q2 = new Query().WithAll<Tag, Transform, Collider>();

        _collisions = [];
    }
    
    /// <summary>
    /// Execute code before updating a <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to update.</param>
    public override void BeforeUpdate(Map map)
    {
        map.Grid.Clear();
    }

    /// <summary>
    /// Update a <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Map map, Time delta)
    {
        // Compute moving object positions based on destination and velocity of the moving object. 
        // We leverage interpolation here to move the object towards the destination over time.

        map.Dynamic(_q1, (Future future, in (Entity, Chunk) _, in Entity entity) => {
            ref var tag = ref map.Space.Get<Tag>(entity);
            ref var transform = ref map.Space.Get<Transform>(entity);

            if (map.Space.Has<Destination>(entity) && map.Space.Has<Velocity>(entity))
            {
                var mover = map.ObjectAt(entity);
                var destination = map.Space.Get<Destination>(entity);
                var velocity = map.Space.Get<Velocity>(entity);
                var position = transform + velocity * (float) delta.Seconds;
                var rotation = Transform.Heading(transform, destination);

                // If the object is close enough to its destination, snap it to the position.
                // Also, stop moving by removing the object's velocity.

                if (Point2D.Distance(transform.X, transform.Y, destination.X, destination.Y) <= 50)
                {
                    mover.Stop(position = transform with { X = destination.X, Y = destination.Y }, future);
                }

                transform = position with { R = rotation };

                Log.Debug("{Object} moved to {Transform}, {Map}.", mover, transform, map.Name);
            }

            map.Grid.Set(entity, transform);
        });

        // After moving objects in the map, detect collisions.
        // This is done after computing object positions to ensure collisions are based on the latest data.

        var collisions = (Current: new ConcurrentHashSet<Pair>(), Existing: new ConcurrentHashSet<Pair>(_collisions));

        map.Dynamic(_q2, (Future future, in (Entity Entity, Chunk Coordinates) chunk, in Entity entity) => {
            var ta = map.Space.Get<Transform>(entity);
            var ca = map.Space.Get<Collider>(entity);

            // Query nearby entities in a grid around the current chunk.
            // This minimizes collision checks to only relevant candidate objects.

            foreach (var candidate in map.Grid.Query(ta, map.Data.Info.Sight))
            {
                if (candidate != entity && map.Space.Has<Collider>(candidate))
                {
                    var tb = map.Space.Get<Transform>(candidate);
                    var cb = map.Space.Get<Collider>(candidate);

                    // Calculate distance between object centers and compare to combined collider size.
                    // Using simple circle collision detection for initial implementation.

                    if (Point2D.Distance(ta.X, ta.Y, tb.X, tb.Y) <= ca.Size + cb.Size)
                    {
                        collisions.Current.Add(new Pair(entity, candidate));
                    }
                }
            }
        });

        foreach (var pair in collisions.Current)
        {
            if (!collisions.Existing.Contains(pair) && map.Space.Exists(pair.First) && map.Space.Exists(pair.Second))
            {
                _collisions.Add(pair);

                var first = map.ObjectAt(pair.First);
                var second = map.ObjectAt(pair.Second);

                first.Enter(second);
                second.Enter(first);

                Log.Debug("{Object} colliding with {Other}.", first, second);
            }
        }

        foreach (var pair in collisions.Existing)
        {
            if (!collisions.Current.Contains(pair))
            {
                _collisions.Remove(pair);

                if (map.Space.Exists(pair.First) && map.Space.Exists(pair.Second))
                {
                    var first = map.ObjectAt(pair.First);
                    var second = map.ObjectAt(pair.Second);

                    first.Exit(second);
                    second.Exit(first);

                    Log.Debug("{Object} no longer colliding with {Other}.", first, second);
                }
            }
        }
    }
}