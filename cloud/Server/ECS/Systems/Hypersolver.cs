// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Neurons;
using Spatial.Cloud.ECS.Components;
using Spatial.Extensions;
using Spatial.Persistence;
using Spatial.Simulation;
using System.Collections.Concurrent;

namespace Spatial.Cloud.ECS.Systems;

/// <summary>
/// A neural network leveraging temporal dynamics for continuous state changes over time.
/// </summary>
[Run(1)]
public class Hypersolver : System
{
    private readonly HypersolverConfiguration _config;

    // Maps: External -> Internal
    // Database records mapped to physical entities.

    private readonly ConcurrentDictionary<string, Entity> _neuronsById;
    private readonly ConcurrentDictionary<string, Entity> _synapsesById;

    // Maps: Internal -> External
    // Physical entities mapped to database records.

    private readonly ConcurrentDictionary<Entity, Data.Neurons.Neuron> _neuronsByEntity;
    private readonly ConcurrentDictionary<Entity, Data.Synapses.Synapse> _synapsesByEntity;

    // Spatial queries for selecting neurons and synapses at runtime.
    // Used below for network updates, but also for bulk writes to the database.

    private readonly Query _neurons;
    private readonly Query _synapses;
    
    // Accumulated pre-synaptic charges.
    // Keyed by post-synaptic neurons for integration later.

    private readonly ConcurrentDictionary<Entity, double> _inputs;
    private readonly ConcurrentDictionary<Entity, double> _rewards;

    /// <summary>
    /// Create a new <see cref="Hypersolver"/>.
    /// </summary>
    public Hypersolver()
    {
        Server.Current.Hypersolver = this;

        _config = ServerConfiguration.Current.Baymax.Hypersolver;

        _neuronsById = [];
        _synapsesById = [];
        _neuronsByEntity = [];
        _synapsesByEntity = [];

        _neurons = new Query().WithAll<Components.Neuron>();
        _synapses = new Query().WithAll<Synapse>();

        _inputs = [];
        _rewards = [];
    }

    /// <summary>
    /// Reward a <see cref="Components.Neuron"/>.
    /// </summary>
    /// <param name="neuron">The <see cref="Components.Neuron"/> to reward.</param>
    /// <param name="signal">A reward signal.</param>
    public void Reward(string neuron, double signal) => Reward(_neuronsById[neuron], signal);

    /// <summary>
    /// Reward a <see cref="Components.Neuron"/>.
    /// </summary>
    /// <param name="neuron">The <see cref="Components.Neuron"/> to reward.</param>
    /// <param name="signal">A reward signal.</param>
    public void Reward(Entity neuron, double signal)
    {
        _rewards.AddOrUpdate(neuron, signal, (_, value) => value + signal);
    }

    /// <summary>
    /// Create the <see cref="Hypersolver"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void Create(Space space)
    {
        // When the Hypersolver is created, reconstruct the brain.
        // Load neural records from the database and replicate in space.

        var neurons = Resource<Data.Neurons.Neuron>.List();
        var synapses = Resource<Data.Synapses.Synapse>.List();

        space.Reserve(Signature.Combine<Components.Neuron, Position>(), (uint) neurons.Count);
        space.Reserve(Signature.Combine<Synapse>(), (uint) synapses.Count);

        for (var i = 0; i < neurons.Count; i++)
        {
            var record = neurons[i];
            var entity = space.Create(
                new Components.Neuron(record.Type, record.Group, record.Channel, record.Value),
                new Position(record.Position.X, record.Position.Y, record.Position.Z));

            _neuronsById[(_neuronsByEntity[entity] = record).Id] = entity;
        }

        for (var i = 0; i < synapses.Count; i++)
        {
            var record = synapses[i];
            var entity = space.Create(new Components.Synapse(
                From: _neuronsById[record.From],
                To: _neuronsById[record.To],
                Strength: record.Strength));

            _synapsesById[(_synapsesByEntity[entity] = record).Id] = entity;
        }
    }

    /// <summary>
    /// Update the <see cref="Hypersolver"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        space.Mutate(_synapses, (Future future, in Entity entity, ref Synapse synapse) => {

            // Synaptic propagation.
            // Accumulate pre-synaptic charges for integration.

            var pre = space.Get<Components.Neuron>(synapse.From);
            var post = space.Get<Components.Neuron>(synapse.To);
            var contribution = pre.Value * synapse.Strength;

            _inputs.AddOrUpdate(synapse.To, contribution, (entity, value) => value + contribution);
            
            // Hebbian plasticity with spatial modulation.
            // Neurons that fire together, wire together.

            var a = space.Get<Position>(synapse.From);
            var b = space.Get<Position>(synapse.To);
            var d = (X: a.X - b.X, Y: a.Y - b.Y, Z: a.Z - b.Z);

            var distance = Math.Sqrt(d.X * d.X + d.Y * d.Y + d.Z * d.Z);
            var activity = pre.Value * post.Value;

            synapse.Strength +=
                _config.Eta * // Synaptic learning rate
                (activity - post.Value * post.Value * synapse.Strength) * // Hebbian plasticity
                (1.0 - Math.Exp(-Math.Abs(activity) / _config.Kappa)) * // Plastic sensitivity
                (1.0 + Math.Clamp(_rewards.GetValueOrDefault(synapse.To), -1.0D, 1.0D)) * // Reward modulation
                Math.Exp(-distance * distance / (2.0 * _config.Sigma * _config.Sigma)) * // Spatial modulation
                delta.Seconds;

            synapse.Strength = Math.Clamp(synapse.Strength, -_config.Omax, _config.Omax);

        });

        space.Mutate(_neurons, (Future future, in Entity entity, ref Components.Neuron neuron) => {

            var input = _inputs.GetValueOrDefault(entity);

            switch (neuron.Type)
            {
                case NeuronType.Temporal:

                    // Temporal state dynamics!
                    // Using Runge-Kutta (RK4) integration, update the neuron.

                    var k1 = (-neuron.Value + Math.Tanh(input)) / _config.Taun;
                    var k2 = (-(neuron.Value + 0.5D * k1 * delta.Seconds) + Math.Tanh(input)) / _config.Taun;
                    var k3 = (-(neuron.Value + 0.5D * k2 * delta.Seconds) + Math.Tanh(input)) / _config.Taun;
                    var k4 = (-(neuron.Value + k3 * delta.Seconds) + Math.Tanh(input)) / _config.Taun;

                    neuron.Value += delta.Seconds / 6.0D * (k1 + 2 * k2 + 2 * k3 + k4);

                    break;

                case NeuronType.Command:

                    // Post-processing using a continuous-time exponential filter.
                    // Smooth and combine temporal state into higher-level signals.

                    var alpha = 1.0D - Math.Exp(-delta.Seconds / _config.Tauf);

                    neuron.Value = (1.0D - alpha) * neuron.Value + alpha * input;

                    break;

                case NeuronType.Motor:

                    // Behavior control, yeah! \o/
                    // Route the motor neuron's output value to its parent module.

                    Server.Current.Transducers.GetValueOrDefault(neuron.Group)?.Apply(neuron.Channel, neuron.Value = Math.Tanh(input));

                    break;
            }
        });
    }

    /// <summary>
    /// Clean up the <see cref="Hypersolver"/> after updating.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void AfterUpdate(Space space)
    {
        _inputs.Clear();
        _rewards.Clear();
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
        space.Mutate(_neurons, (Future future, in Entity entity) => SaveNeuron(space, entity));
        space.Mutate(_synapses, (Future future, in Entity entity) => SaveSynapse(space, entity));
    }

    private void SaveNeuron(Space space, Entity entity) => _neuronsByEntity[entity].Update(record => {
        var neuron = space.Get<Components.Neuron>(entity);
        var position = space.Get<Position>(entity);

        record.Position.X = position.X;
        record.Position.Y = position.Y;
        record.Position.Z = position.Z;

        record.Value = neuron.Value;
    });

    private void SaveSynapse(Space space, Entity entity) => _synapsesByEntity[entity].Update(record => {
        record.Strength = space.Get<Synapse>(entity).Strength;
    });
}

/// <summary>
/// Configurable options for the <see cref="Hypersolver"/>.
/// </summary>
public class HypersolverConfiguration
{
    /// <summary>
    /// The sensitivity or gain of plasticity.
    /// </summary>
    public double Kappa { get; set; } = 0.5D;

    /// <summary>
    /// The learning rate of the <see cref="Hypersolver"/>.
    /// </summary>
    public double Eta { get; set; } = 0.001D;

    /// <summary>
    /// A time-constant used for integration, in seconds.
    /// </summary>
    public double Taun { get; set; } = 0.08D;

    /// <summary>
    /// A time-constant used for filtering, in seconds.
    /// </summary>
    public double Tauf { get; set; } = 0.05D;

    /// <summary>
    /// The maximum synaptic strength.
    /// </summary>
    public double Omax { get; set; } = 2.0D;

    /// <summary>
    /// The spatial falloff radius for synaptic plasticity.
    /// </summary>
    public double Sigma { get; set; } = 50.0D;
}