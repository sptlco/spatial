// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Agents;
using Spatial.Cloud.Components;
using Spatial.Compute;
using Spatial.Simulation;
using System.Collections.Concurrent;

namespace Spatial.Cloud.Systems;

/// <summary>
/// A proprietary feature extractor.
/// </summary>
[Dependency(0)]
public class Extractor : System
{
    private readonly Query _query;

    private readonly ConcurrentDictionary<int, double[]> _inputs;
    private readonly ConcurrentDictionary<int, double[]> _features;

    /// <summary>
    /// Create a new <see cref="Extractor"/>.
    /// </summary>
    public Extractor()
    {
        Server.Current.Extractor = this;

        _query = new Query().WithAll<Neuron>();

        _inputs = [];
        _features = [];
    }
 
    /// <summary>
    /// Present raw data to an <see cref="Agent"/>.
    /// </summary>
    /// <param name="group">A group identification number.</param>
    /// <param name="data">Raw data from the input stream.</param>
    public void Set(int group, double[] data)
    {
        _inputs[group] = data;
    }

    /// <summary>
    /// Prepare the <see cref="Extractor"/> before updating.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void BeforeUpdate(Space space)
    {
        // Feature extraction.
        // Pre-process the raw data from the input stream.

        Job.ParallelFor(_inputs, (agent, data) => _features[agent] = Server.Current.Agents[agent].Extract(data)).Wait();

        // Feature passing.
        // Use the extracted feature vector to update the sensory neurons.

        space.Mutate(
            query: _query,
            filter: (_, neuron) => neuron.Type == NeuronType.Sensory,
            function: (Future future, in Entity entity, ref Neuron neuron) => {
                neuron.Value = Math.Tanh(_features.GetValueOrDefault(neuron.Group)?[neuron.Channel] ?? 0.0D);
            });
    }

    /// <summary>
    /// Clean up the <see cref="Extractor"/> after updating.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void AfterUpdate(Space space)
    {
        _inputs.Clear();
        _features.Clear();
    }
}