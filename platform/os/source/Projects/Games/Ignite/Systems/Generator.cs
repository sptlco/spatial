// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Ignite.Models;
using Serilog;
using Spatial.Compute.Jobs;
using Spatial.Simulation;
using Spatial.Structures;

namespace Ignite.Systems;

/// <summary>
/// A <see cref="System"/> that dynamically loads and unloads <see cref="Map"/> chunks.
/// </summary>
public class Generator : System
{
    private readonly Query _q1;
    private readonly Query _q2;

    /// <summary>
    /// Create a new <see cref="Generator"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> controlled by the <see cref="System"/>.</param>
    public Generator(Map map) : base(map)
    {
        _q1 = new Query().WithAll<Player>();
        _q2 = new Query().WithAll<Chunk>().WithNone<Disabled>();
    }

    /// <summary>
    /// Update a <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The <see cref="Map"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Map map, Time delta)
    {
        // Using a hash set, track chunks that should be active.
        // Query the map for active chunks based on player locations.

        var chunks = new ConcurrentHashSet<uint>();

        map.Space.Mutate(_q1, (Future future, in Entity entity, ref Player player, ref Transform transform) => {
            foreach (var chunk in map.Grid.View(transform))
            {
                chunks.Add(chunk);
            }
        });

        // Now that we know which chunks should be active, we can unload 
        // all of those that should not be (because they are not in the hash set).

        map.Space.Mutate(_q2, (Future future, in Entity entity, ref Chunk chunk) => {
            if (!chunks.Contains(entity))
            {
                // Unload the chunk since there are no players near it.
                // Since the query contains chunks without the disabled component, 
                // we are sure here that the chunk is currently loaded.

                future.Add<Disabled>(entity);

                Log.Debug("Unloaded chunk {Chunk} at {Position}, {Map}.", chunk, map.Space.Get<Transform>(entity), map.Name);
            }
        });

        // Lastly, we load each chunk in the hash set.
        // These are chunks with nearby players, and are loaded in parallel.

        Job.ParallelFor(chunks, entity => {
            if (map.Space.Has<Disabled>(entity))
            {
                // If the chunk is disabled, or not loaded, then we load the chunk, 
                // since there are players near it (it's in the hash set).

                map.Space.Future.Remove<Disabled>(entity);

                Log.Debug("Loaded chunk {Chunk} at {Position}, {Map}.", map.Space.Get<Chunk>(entity), map.Space.Get<Transform>(entity), map.Name);
            }
        });

        map.Space.Future.Commit(map.Space);
    }
}