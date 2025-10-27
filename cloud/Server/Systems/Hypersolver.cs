// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Components;
using Spatial.Cloud.Models.Nodes;
using Spatial.Persistence;
using Spatial.Simulation;
using System.Collections.Concurrent;

namespace Spatial.Cloud.Systems;

/// <summary>
/// ...
/// </summary>
[Dependency]
public class Hypersolver : System
{
    private readonly ConcurrentDictionary<string, (Node Record, Entity Entity)> _nodes;
    private readonly ConcurrentDictionary<string, (Connection Record, Entity Entity)> _connections;

    private readonly Query _neurons;
    private readonly Query _synapses;

    /// <summary>
    /// Create a new <see cref="Hypersolver"/>.
    /// </summary>
    public Hypersolver()
    {
        _nodes = [];
        _connections = [];

        _neurons = new Query().WithAll<Neuron>();
        _synapses = new Query().WithAll<Synapse>();
    }

    /// <summary>
    /// Create the <see cref="Hypersolver"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void Create(Space space)
    {
        // When the Hypersolver is created, reconstruct the brain.
        // Load neural records from the database and replicate in space.

        var nodes = Record<Node>.List();
        var connections = Record<Connection>.List();

        for (var i = 0; i < nodes.Count; i++)
        {
            var record = nodes[i];

            _nodes[record.Id] = (record, space.Create(
                new Neuron(record.Value),
                new Position(record.Position.X, record.Position.Y, record.Position.Z),
                new Rotation(record.Rotation.X, record.Rotation.Y, record.Rotation.Z)));
        }

        for (var i = 0; i < connections.Count; i++)
        {
            var record = connections[i];

            _connections[record.Id] = (record, space.Create(new Synapse(
                From: _nodes[record.From].Entity,
                To: _nodes[record.To].Entity,
                Strength: record.Strength)));
        }
    }

    /// <summary>
    /// Update the <see cref="Hypersolver"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        // ...

        space.Mutate(_synapses, (Future future, in Entity entity, ref Synapse synapse) => {

            // ...

        });

        space.Mutate(_neurons, (Future future, in Entity entity, ref Neuron neuron) => {

            // ...

        });
    }

    /// <summary>
    /// Destroy the <see cref="Hypersolver"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void Destroy(Space space)
    {
        space.Mutate(_neurons, (Future future, in Entity entity, ref Neuron neuron, ref Position position, ref Rotation rotation) => {
            
        });
    }
}