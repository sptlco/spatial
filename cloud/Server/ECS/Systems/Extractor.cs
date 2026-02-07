// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Neurons;
using Spatial.Compute;
using Spatial.Simulation;
using System.Collections.Concurrent;

using Neuron = Spatial.Cloud.ECS.Components.Neuron;

namespace Spatial.Cloud.ECS.Systems;

/// <summary>
/// A proprietary feature extractor.
/// </summary>
[Run(0)]
public class Extractor : System
{
    private readonly Query _query;
    private readonly ConcurrentDictionary<int, double[]> _inputs;

    /// <summary>
    /// Create a new <see cref="Extractor"/>.
    /// </summary>
    public Extractor()
    {
        Server.Current.Extractor = this;

        _query = new Query().WithAll<Neuron>();
        _inputs = [];
    }

    /// <summary>
    /// Prepare the <see cref="Extractor"/> before updating.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void BeforeUpdate(Space space)
    {
        // Feature extraction.
        // Pre-process the inputs.

        Job.ParallelFor(Server.Current.Propagators, (channel, propagator) => _inputs[channel] = propagator.Extract()).Wait();

        // Feature passing.
        // Use the extracted feature vector to update the sensory neurons.

        space.Mutate(
            query: _query,
            filter: (_, neuron) => neuron.Type == NeuronType.Sensory,
            function: (Future future, in Entity entity, ref Neuron neuron) => {
                neuron.Value = Math.Tanh(_inputs.GetValueOrDefault(neuron.Protocol)?[neuron.Channel] ?? 0.0D);
            });
    }

    /// <summary>
    /// Clean up the <see cref="Extractor"/> after updating.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void AfterUpdate(Space space)
    {
        _inputs.Clear();
    }
}