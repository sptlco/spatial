// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Intelligence;
using Spatial.Cloud.Data.Intelligence.Neurons;
using Spatial.Cloud.Data.Intelligence.Synapses;
using Spatial.Cloud.ECS.Components;
using Spatial.Extensions;
using Spatial.Persistence;
using Spatial.Simulation;
using Spatial.Structures;
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

    private readonly ConcurrentDictionary<Entity, Data.Intelligence.Neurons.Neuron> _neuronsByEntity;
    private readonly ConcurrentDictionary<Entity, Data.Intelligence.Synapses.Synapse> _synapsesByEntity;

    // Reverse index.
    // Maps neuron IDs to the synapse IDs that reference them.

    private readonly ConcurrentDictionary<string, ConcurrentHashSet<string>> _synapsesByNeuron;

    // Spatial queries for selecting neurons and synapses at runtime.
    // Used below for network updates, but also for bulk writes to the database.

    private readonly Query _neurons;
    private readonly Query _synapses;

    // Deferred commands.
    // Drained at the start of each tick.

    private readonly ConcurrentQueue<Action<Space>> _commands;
    
    // Accumulated pre-synaptic charges.
    // Keyed by post-synaptic neurons for integration later.

    private readonly ConcurrentDictionary<Entity, double> _inputs;

    // State propagation.
    // Publish a snapshot of the current state of the network.

    private Snapshot _front;
    private Snapshot _back;

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
        _synapsesByNeuron = [];

        _front = new Snapshot();
        _back = new Snapshot();

        _neurons = new Query().WithAll<Components.Neuron>();
        _synapses = new Query().WithAll<Components.Synapse>();

        _commands = [];
        _inputs = [];
    }

    /// <summary>
    /// Get the current state of the <see cref="Hypersolver"/>.
    /// </summary>
    public Snapshot Snapshot => Volatile.Read(ref _front);

    /// <summary>
    /// The neural network's topology.
    /// </summary>
    public (IEnumerable<Data.Intelligence.Neurons.Neuron> Neurons, IEnumerable<Data.Intelligence.Synapses.Synapse> Synapses) Resources => (_neuronsByEntity.Values, _synapsesByEntity.Values);

    /// <summary>
    /// An <see cref="event"/> fired when the network is updated.
    /// </summary>
    public event Action<Snapshot>? Updated;

    /// <summary>
    /// Events for neuron lifecycle and state changes.
    /// </summary>
    public NeuronEvents Neurons { get; } = new NeuronEvents();

    /// <summary>
    /// Events for synapse lifecycle and state changes.
    /// </summary>
    public SynapseEvents Synapses { get; } = new SynapseEvents();

    /// <summary>
    /// Create the <see cref="Hypersolver"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void Create(Space space)
    {
        // When the Hypersolver is created, reconstruct the brain.
        // Load neural records from the database and replicate in space.

        var neurons = Resource<Data.Intelligence.Neurons.Neuron>.List();
        var synapses = Resource<Data.Intelligence.Synapses.Synapse>.List();

        space.Reserve(Signature.Combine<Components.Neuron, Position>(), (uint) neurons.Count);
        space.Reserve(Signature.Combine<Components.Synapse>(), (uint) synapses.Count);

        for (var i = 0; i < neurons.Count; i++)
        {
            CreateNeuron(space, neurons[i]);
        }

        for (var i = 0; i < synapses.Count; i++)
        {
            CreateSynapse(space, synapses[i]);
        }
    }

    /// <summary>
    /// Prepare the <see cref="Hypersolver"/> before updating.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void BeforeUpdate(Space space)
    {
        while (_commands.TryDequeue(out var command))
        {
            command(space);
        }
    }

    /// <summary>
    /// Update the <see cref="Hypersolver"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        space.Mutate(_synapses, (Future future, in Entity entity, ref Components.Synapse synapse) => {

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
                Math.Exp(-distance * distance / (2.0 * _config.Sigma * _config.Sigma)) * // Spatial modulation
                delta.Seconds;

            _synapsesByEntity[entity].Strength = synapse.Strength = Math.Clamp(synapse.Strength, -_config.Omax, _config.Omax);
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

                    Server.Current.Behaviors.GetValueOrDefault(neuron.Group)?.Apply(neuron.Channel, neuron.Value = Math.Tanh(input));

                    break;
            }

            _neuronsByEntity[entity].Value = neuron.Value; 
        });
    }

    /// <summary>
    /// Clean up the <see cref="Hypersolver"/> after updating.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void AfterUpdate(Space space)
    {
        _inputs.Clear();

        _back.Neurons  = [.. _neuronsByEntity.Values.Select(n => new NeuronSnapshot(n.Id, n.Type, n.Group, n.Channel, n.Position, n.Value))];
        _back.Synapses = [.. _synapsesByEntity.Values.Select(s => new SynapseSnapshot(s.Id, s.From, s.To, s.Strength))];
        _back = Interlocked.Exchange(ref _front, _back);

        Interval.ScheduleAsync(SaveAsync, Time.FromMinutes(5));

        Updated?.Invoke(_front);
    }

    /// <summary>
    /// Stimulate a <see cref="Data.Intelligence.Neurons.Neuron"/>.
    /// </summary>
    /// <param name="neuron">The <see cref="Data.Intelligence.Neurons.Neuron"/> to stimulate.</param>
    /// <param name="charge">A stimulus for the <see cref="Data.Intelligence.Neurons.Neuron"/>.</param>
    /// <exception cref="UserError">Thrown if the <see cref="Data.Intelligence.Neurons.Neuron"/> does not exist.</exception>
    public void StimulateOne(string neuron, double charge)
    {
        if (!_neuronsById.TryGetValue(neuron, out var entity))
        {
            throw new UserError("The neuron does not exist.");
        }

        _inputs.AddOrUpdate(entity, charge, (_, existing) => existing + charge);
    }

    /// <summary>
    /// Stimulate several neurons.
    /// </summary>
    /// <param name="neurons">The neurons to stimulate.</param>
    /// <param name="charge">A stimulus for the neurons.</param>
    /// <exception cref="UserError">Thrown if any of the neurons do not exist.</exception>
    public void StimulateMany(string[] neurons, double charge)
    {
        var failures = 0;

        foreach (var neuron in neurons)
        {
            try
            {
                StimulateOne(neuron, charge);
            }
            catch (Exception)
            {
                failures++;
            }
        }

        if (failures > 0)
        {
            throw new UserError($"Failed to stimulate {failures}/{neurons.Length} neurons.");
        }
    }

    /// <summary>
    /// Add a <see cref="Data.Intelligence.Neurons.Neuron"/> to the network.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Data.Intelligence.Neurons.Neuron"/>.</param>
    /// <returns>The <see cref="Data.Intelligence.Neurons.Neuron"/> that was added.</returns>
    public Data.Intelligence.Neurons.Neuron AddNeuron(CreateNeuronOptions options)
    {
        var record = new Data.Intelligence.Neurons.Neuron {
            Type = options.Type,
            Group = options.Group,
            Channel = options.Channel,
            Position = options.Position,
            Value = options.Value
        };

        _commands.Enqueue(space => CreateNeuron(space, record, true));

        return record.Store();
    }

    /// <summary>
    /// Update a <see cref="Data.Intelligence.Neurons.Neuron"/>.
    /// </summary>
    /// <param name="neuron">The <see cref="Data.Intelligence.Neurons.Neuron"/> to update.</param>
    /// <param name="options">Configurable options for the <see cref="Data.Intelligence.Neurons.Neuron"/>.</param>
    /// <exception cref="UserError">Thrown if the <see cref="Data.Intelligence.Neurons.Neuron"/> does not exist.</exception>
    public void UpdateNeuron(string neuron, UpdateNeuronOptions options)
    {
        if (!_neuronsById.TryGetValue(neuron, out var entity))
        {
            throw new UserError("The neuron does not exist.");
        }

        _commands.Enqueue(space => {
            ref var component = ref space.Get<Components.Neuron>(entity);
            ref var position = ref space.Get<Position>(entity);

            var record = _neuronsByEntity[entity];
            
            component.Type = record.Type = options.Type ?? record.Type;
            component.Group = record.Group = options.Group ?? record.Group;
            component.Channel = record.Channel = options.Channel ?? record.Channel;

            position.X = record.Position.X = options.Position?.X ?? record.Position.X;
            position.Y = record.Position.Y = options.Position?.Y ?? record.Position.Y;
            position.Z = record.Position.Z = options.Position?.Z ?? record.Position.Z;

            Task.Run(() => record.SaveAsync()).ContinueWith(
                continuationAction: t => ERROR(t.Exception, "Failed to persist neuron {Id}.", record.Id),
                continuationOptions: TaskContinuationOptions.OnlyOnFaulted);

            Neurons.InvokeUpdated(record);
        });
    }

    /// <summary>
    /// Remove a <see cref="Data.Intelligence.Neurons.Neuron"/> from the network.
    /// </summary>
    /// <param name="neuron">The <see cref="Data.Intelligence.Neurons.Neuron"/> to remove.</param>
    /// <exception cref="UserError">Thrown if the <see cref="Data.Intelligence.Neurons.Neuron"/> does not exist.</exception>
    public void RemoveNeuron(string neuron)
    {
        if (!_neuronsById.TryGetValue(neuron, out var entity))
        {
            throw new UserError("The neuron does not exist.");
        }

        if (_synapsesByNeuron.TryGetValue(neuron, out var connected))
        {
            connected.ToList().ForEach(RemoveSynapse);

            _synapsesByNeuron.TryRemove(neuron, out _);
        }

        var record = _neuronsByEntity[entity];

        Resource<Data.Intelligence.Neurons.Neuron>.RemoveOne(n => n.Id == neuron);

        _commands.Enqueue(space => {
            space.Destroy(entity);

            _neuronsById.TryRemove(neuron, out _);
            _neuronsByEntity.TryRemove(entity, out _);
            
            Neurons.InvokeRemoved(record);
        });
    }

    /// <summary>
    /// Add a <see cref="Data.Intelligence.Synapses.Synapse"/> to the network.
    /// </summary>
    /// <param name="options">Configurable options for the <see cref="Data.Intelligence.Synapses.Synapse"/>.</param>
    /// <returns>The <see cref="Data.Intelligence.Synapses.Synapse"/> that was added.</returns>
    /// <exception cref="UserError">Thrown if either the pre-synaptic or post-synaptic <see cref="Data.Intelligence.Neurons.Neuron"/> does not exist.</exception>
    public Data.Intelligence.Synapses.Synapse AddSynapse(CreateSynapseOptions options)
    {
        if (!_neuronsById.ContainsKey(options.From))
        {
            throw new UserError("The pre-synaptic neuron does not exist.");
        }

        if (!_neuronsById.ContainsKey(options.To))
        {
            throw new UserError("The post-synaptic neuron does not exist.");
        }

        var record = new Data.Intelligence.Synapses.Synapse {
            From = options.From,
            To = options.To,
            Strength = options.Strength
        };

        _commands.Enqueue(space => CreateSynapse(space, record, true));

        return record.Store();
    }

    /// <summary>
    /// Update a <see cref="Data.Intelligence.Synapses.Synapse"/>.
    /// </summary>
    /// <param name="synapse">The <see cref="Data.Intelligence.Synapses.Synapse"/> to update.</param>
    /// <param name="options">Configurable options for the <see cref="Data.Intelligence.Synapses.Synapse"/>.</param>
    /// <exception cref="UserError">Thrown if either the <see cref="Data.Intelligence.Neurons.Neuron"/> doesn't exist, or its pre-synaptic or post-synaptic <see cref="Data.Intelligence.Neurons.Neuron"/> doesn't exist.</exception>
    public void UpdateSynapse(string synapse, UpdateSynapseOptions options)
    {
        if (!_synapsesById.TryGetValue(synapse, out var entity))
        {
            throw new UserError("The synapse does not exist.");
        }

        if (options.From is not null && !_neuronsById.ContainsKey(options.From))
        {
            throw new UserError("The pre-synaptic neuron does not exist.");
        }

        if (options.To is not null && !_neuronsById.ContainsKey(options.To))
        {
            throw new UserError("The post-synaptic neuron does not exist.");
        }

        _commands.Enqueue(space => {
            ref var component = ref space.Get<Components.Synapse>(entity);

            var record = _synapsesByEntity[entity];

            if (options.From is string from)
            {
                if (_synapsesByNeuron.TryGetValue(record.From, out var old))
                {
                    old.Remove(synapse);
                }

                _synapsesByNeuron.GetOrAdd(from, _ => []).Add(synapse);

                component.From = _neuronsById[from];
                record.From = from;
            }

            if (options.To is string to)
            {
                if (_synapsesByNeuron.TryGetValue(record.To, out var old))
                {
                    old.Remove(synapse);
                }

                _synapsesByNeuron.GetOrAdd(to, _ => []).Add(synapse);

                component.To = _neuronsById[to];
                record.To = to;
            }

            component.Strength = record.Strength = options.Strength ?? record.Strength;

            Task.Run(() => record.SaveAsync()).ContinueWith(
                continuationAction: t => ERROR(t.Exception, "Failed to persist synapse {Id}.", record.Id),
                continuationOptions: TaskContinuationOptions.OnlyOnFaulted);

            Synapses.InvokeUpdated(record);
        });
    }

    /// <summary>
    /// Remove a <see cref="Data.Intelligence.Synapses.Synapse"/> from the network.
    /// </summary>
    /// <param name="synapse">The <see cref="Data.Intelligence.Synapses.Synapse"/> to remove.</param>
    /// <exception cref="UserError">Thrown if the <see cref="Data.Intelligence.Synapses.Synapse"/> does not exist.</exception>
    public void RemoveSynapse(string synapse)
    {
        if (!_synapsesById.TryGetValue(synapse, out var entity))
        {
            throw new UserError("The synapse does not exist.");
        }

        var record = _synapsesByEntity[entity];

        Resource<Data.Intelligence.Synapses.Synapse>.RemoveOne(s => s.Id == synapse);

        if (_synapsesByNeuron.TryGetValue(record.From, out var from))
        {
            from.Remove(synapse);
        }

        if (_synapsesByNeuron.TryGetValue(record.To, out var to))
        {
            to.Remove(synapse);
        }

        _commands.Enqueue(space => {
            space.Destroy(entity);

            _synapsesById.TryRemove(synapse, out _);
            _synapsesByEntity.TryRemove(entity, out _);
            
            Synapses.InvokeRemoved(record);
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

        Task.WaitAll(SaveAsync());
    }

    private void CreateNeuron(Space space, Data.Intelligence.Neurons.Neuron record, bool notify = false)
    {
        var entity = space.Create(
            new Components.Neuron(record.Type, record.Group, record.Channel, record.Value),
            new Position(record.Position.X, record.Position.Y, record.Position.Z));

        _neuronsById[(_neuronsByEntity[entity] = record).Id] = entity;

        if (notify)
        {
            Neurons.InvokeAdded(record);
        }
    }

    private void CreateSynapse(Space space, Data.Intelligence.Synapses.Synapse record, bool notify = false)
    {
        var entity = space.Create(new Components.Synapse(
            From: _neuronsById[record.From],
            To: _neuronsById[record.To],
            Strength: record.Strength));

        _synapsesById[(_synapsesByEntity[entity] = record).Id] = entity;

        _synapsesByNeuron.GetOrAdd(record.From, _ => []).Add(record.Id);
        _synapsesByNeuron.GetOrAdd(record.To, _ => []).Add(record.Id);

        if (notify)
        {
            Synapses.InvokeAdded(record);
        }
    }

    private Task SaveAsync()
    {
        return Task.WhenAll(SaveNeurons(), SaveSynapses());
    }

    private Task SaveNeurons()
    {
        return !_neuronsByEntity.IsEmpty ? Resource<Data.Intelligence.Neurons.Neuron>.ReplaceManyAsync(_neuronsByEntity.Values) : Task.CompletedTask;
    }

    private Task SaveSynapses()
    {
        return !_synapsesByEntity.IsEmpty ? Resource<Data.Intelligence.Synapses.Synapse>.ReplaceManyAsync(_synapsesByEntity.Values) : Task.CompletedTask;
    }
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

/// <summary>
/// Events for neuron lifecycle and state changes.
/// </summary>
public class NeuronEvents
{
    /// <summary>
    /// Fired when a neuron is added to the network.
    /// </summary>
    public event Action<Data.Intelligence.Neurons.Neuron>? Added;

    /// <summary>
    /// Fired when a neuron is externally mutated.
    /// </summary>
    public event Action<Data.Intelligence.Neurons.Neuron>? Updated;

    /// <summary>
    /// Fired when a neuron is removed from the network.
    /// </summary>
    public event Action<Data.Intelligence.Neurons.Neuron>? Removed;

    internal void InvokeAdded(Data.Intelligence.Neurons.Neuron neuron)   => Added?.Invoke(neuron);
    internal void InvokeUpdated(Data.Intelligence.Neurons.Neuron neuron) => Updated?.Invoke(neuron);
    internal void InvokeRemoved(Data.Intelligence.Neurons.Neuron neuron) => Removed?.Invoke(neuron);
}

/// <summary>
/// Events for synapse lifecycle and state changes.
/// </summary>
public class SynapseEvents
{
    /// <summary>
    /// Fired when a synapse is added to the network.
    /// </summary>
    public event Action<Data.Intelligence.Synapses.Synapse>? Added;

    /// <summary>
    /// Fired when a synapse is externally mutated.
    /// </summary>
    public event Action<Data.Intelligence.Synapses.Synapse>? Updated;

    /// <summary>
    /// Fired when a synapse is removed from the network.
    /// </summary>
    public event Action<Data.Intelligence.Synapses.Synapse>? Removed;

    internal void InvokeAdded(Data.Intelligence.Synapses.Synapse synapse)   => Added?.Invoke(synapse);
    internal void InvokeUpdated(Data.Intelligence.Synapses.Synapse synapse) => Updated?.Invoke(synapse);
    internal void InvokeRemoved(Data.Intelligence.Synapses.Synapse synapse) => Removed?.Invoke(synapse);
}