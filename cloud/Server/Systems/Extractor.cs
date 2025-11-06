// Copyright © Spatial Corporation. All rights reserved.

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
    /// Push data to the <see cref="Extractor"/>.
    /// </summary>
    /// <param name="group">A neural group number.</param>
    /// <param name="signals">Input signals for sensory neurons.</param>
    public void Push(int group, double[] signals)
    {
        _inputs.AddOrUpdate(group, signals, (_, value) => AddElementWise(value, signals));
    }

    /// <summary>
    /// Prepare the <see cref="Extractor"/> before updating.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public override void BeforeUpdate(Space space)
    {
        // Feature extraction.
        // Pre-process the inputs.

        Job.ParallelFor(_inputs, (group, data) => _features[group] = Server.Current.Transducers[group].Extract(data)).Wait();

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

    private double[] AddElementWise(double[] a, double[] b)
    {
        if (a.Length != b.Length)
        {
            throw new ArgumentException("The arrays must match in length.");
        }

        for (var i = 0; i < a.Length; i++)
        {
            a[i] += b[i];
        }

        return a;
    }
}