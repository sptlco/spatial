// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Intelligence.Neurons;
using Spatial.Cloud.Data.Intelligence.Synapses;

namespace Spatial.Cloud.Data.Intelligence;

/// <summary>
/// The current state of the <see cref="ECS.Systems.Hypersolver"/>.
/// </summary>
public class Snapshot
{
    /// <summary>
    /// The brain's neurons.
    /// </summary>
    public IReadOnlyList<NeuronSnapshot> Neurons { get; internal set; } = [];

    /// <summary>
    /// The brain's synapses.
    /// </summary>
    public IReadOnlyList<SynapseSnapshot> Synapses { get; internal set; } = [];
}