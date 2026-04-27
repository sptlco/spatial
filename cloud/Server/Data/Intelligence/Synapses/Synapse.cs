// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Data.Intelligence.Synapses;

/// <summary>
/// A weighted connection between two neurons.
/// </summary>
[Collection("synapses")]
public class Synapse : Resource
{
    /// <summary>
    /// The <see cref="Neurons.Neuron"/> the <see cref="Synapse"/> extends from.
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// The <see cref="Neurons.Neuron"/> the <see cref="Synapse"/> extends to.
    /// </summary>
    public string To { get; set; }

    /// <summary>
    /// The strength of the <see cref="Synapse"/>.
    /// </summary>
    public double Strength { get; set; }
}