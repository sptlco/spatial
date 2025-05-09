// Copyright © Spatial. All rights reserved.

using Geneva.Components;
using Geneva.Systems;
using Serilog;
using Spatial.Mathematics;
using Spatial.Simulation;

namespace Geneva.Models;

/// <summary>
/// A unique configuration of biologically designed intelligence.
/// </summary>
public static class Schematic
{
    private static Space _space;

    /// <summary>
    /// Create a new <see cref="Schematic"/>.
    /// </summary>
    /// <param name="sensors">The brain's sensor <see cref="Receptor"/> count.</param>
    /// <param name="layers">The brain's layer shape.</param>
    /// <param name="motors">The brain's motor <see cref="Receptor"/> count.</param>
    public static void Create(int sensors, int[] layers, int motors)
    {
        _space = Space
            .Empty()
            .Use<Integrator>()
            .Use<Regulator>()
            .Use<Tuner>()
            .Use<Caster>();

        for (var i = 0; i < sensors; i++)
        {
            _space.Create(
                new Neuron(Constants.pR),
                new Receptor(ReceptorType.Sensor, i),
                new Layer(LayerType.Input));
        }

        for (var i = 0; i < layers.Length; i++)
        {
            for (var j = 0; j < layers[i]; j++)
            {
                _space.Create(
                    new Neuron(Constants.pR),
                    new Layer(LayerType.Network, i));
            }
        }

        for (var i = 0; i < motors; i++)
        {
            _space.Create(
                new Neuron(Constants.pR),
                new Receptor(ReceptorType.Motor, i),
                new Layer(LayerType.Output));
        }

        var inputs = _space.Query<Layer>(l => l.Type == LayerType.Input);
        var outputs = _space.Query<Layer>(l => l.Type == LayerType.Output);

        if (layers.Length > 0)
        {
            Connect(inputs, GetLayer(0));

            for (var i = 0; i < layers.Length - 1; i++)
            {
                Connect(GetLayer(i), GetLayer(i + 1));
            }

            Connect(GetLayer(layers.Length - 1), outputs);
        }
        else
        {
            Connect(inputs, outputs);
        }
    }

    /// <summary>
    /// Update the <see cref="Schematic"/>.
    /// </summary>
    /// <param name="delta"><see cref="Time"/> passed since the last update.</param>
    public static void Update(Time delta)
    {
        _space.Update(delta);
    }

    /// <summary>
    /// Stream data to a particular channel of the <see cref="Schematic"/>.
    /// </summary>
    /// <param name="channel">The input channel to stimulate.</param>
    /// <param name="intensity">The intensity of the stimulus.</param>
    public static void Stream(int channel, double intensity)
    {
        _space.Mutate(
            query: new Query().WithAll<Neuron, Receptor>(), 
            function: (Future f, in Entity e, ref Neuron n, ref Receptor r) => 
            {
                if (n.State == State.Resting && r.Type == ReceptorType.Sensor && r.Channel == channel)
                {
                    n.Charge += intensity;
                }
            });
    }

    /// <summary>
    /// Stream data to the <see cref="Schematic"/>.
    /// </summary>
    /// <param name="stimuli">A list of stimuli.</param>
    public static void Stream(double[] stimuli)
    {
        _space.Mutate(
            query: new Query().WithAll<Neuron, Receptor>(), 
            function: (Future f, in Entity e, ref Neuron n, ref Receptor r) => 
            {
                if (n.State == State.Resting && r.Type == ReceptorType.Sensor)
                {
                    n.Charge += stimuli[r.Channel];
                }
            });
    }

    private static IEnumerable<Entity> GetLayer(int depth)
    {
        return _space.Query<Layer>(l => l.Type == LayerType.Network && l.Depth == depth);
    }

    private static void Connect(IEnumerable<Entity> sources, IEnumerable<Entity> targets)
    {
        foreach (var source in sources)
        {
            foreach (var target in targets)
            {
                var synapse = _space.Create(new Synapse(
                    Source: source,
                    Destination: target,
                    Strength: Strong.Double(0.0D, 1.0D)
                ));

                Log.Debug("Synapse {synapse} connects {pre} to {post}.", synapse.Id, source.Id, target.Id);
            }
        }
    }
}