// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Components;
using Spatial.Cloud.Models.Nodes;
using Spatial.Extensions;
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
    // Maps: External -> Internal
    // Database records mapped to physical entities.

    private readonly ConcurrentDictionary<string, Entity> _nodes2Neurons;
    private readonly ConcurrentDictionary<string, Entity> _connections2Synapses;

    // Maps: Internal -> External
    // Physical entities mapped to database records.

    private readonly ConcurrentDictionary<Entity, Node> _neurons2Nodes;
    private readonly ConcurrentDictionary<Entity, Connection> _synapses2Connections;

    // Spatial queries for selecting neurons and synapses at runtime.
    // Used below for network updates, but also for bulk writes to the database.

    private readonly Query _neurons;
    private readonly Query _synapses;

    /// <summary>
    /// Create a new <see cref="Hypersolver"/>.
    /// </summary>
    public Hypersolver()
    {
        _nodes2Neurons = [];
        _connections2Synapses = [];
        _neurons2Nodes = [];
        _synapses2Connections = [];

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

        space.Reserve(Signature.Combine<Neuron, Position, Rotation>(), (uint) nodes.Count);
        space.Reserve(Signature.Combine<Synapse>(), (uint) connections.Count);

        for (var i = 0; i < nodes.Count; i++)
        {
            var record = nodes[i];
            var entity = space.Create(
                new Neuron(record.Value),
                new Position(record.Position.X, record.Position.Y, record.Position.Z),
                new Rotation(record.Rotation.X, record.Rotation.Y, record.Rotation.Z));

            _nodes2Neurons[(_neurons2Nodes[entity] = record).Id] = entity;
        }

        for (var i = 0; i < connections.Count; i++)
        {
            var record = connections[i];
            var entity = space.Create(new Synapse(
                From: _nodes2Neurons[record.From],
                To: _nodes2Neurons[record.To],
                Strength: record.Strength));

            _connections2Synapses[(_synapses2Connections[entity] = record).Id] = entity;
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
        // When the space is destroyed (e.g. on shutdown), persist the Hypersolver.
        // Write each neuron and synapse back to the database.

        Save(space);
    }

    private void Save(Space space)
    {
        space.Mutate(_neurons, (Future future, in Entity entity) => SaveNode(space, entity));
        space.Mutate(_synapses, (Future future, in Entity entity) => SaveConnection(space, entity));
    }

    private void SaveNode(Space space, Entity entity) => _neurons2Nodes[entity].Update(record => {
        var neuron = space.Get<Neuron>(entity);
        var position = space.Get<Position>(entity);
        var rotation = space.Get<Rotation>(entity);

        record.Position.X = position.X;
        record.Position.Y = position.Y;
        record.Position.Z = position.Z;

        record.Rotation.X = rotation.X;
        record.Rotation.Y = rotation.Y;
        record.Rotation.Z = rotation.Z;

        record.Value = neuron.Value;
    });

    private void SaveConnection(Space space, Entity entity) => _synapses2Connections[entity].Update(record => {
        record.Strength = space.Get<Synapse>(entity).Strength;
    });
}