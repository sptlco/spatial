// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Brain.Synapses;

/// <summary>
/// Configurable options for a <see cref="Synapse"/> update.
/// </summary>
public class UpdateSynapseOptions
{
    /// <summary>
    /// The <see cref="Neurons.Neuron"/> the <see cref="Synapse"/> extends from.
    public string? From { get; set; }

    /// <summary>
    /// The <see cref="Neurons.Neuron"/> the <see cref="Synapse"/> extends to.
    /// </summary>
    public string? To { get; set; }

    /// <summary>
    /// The current strength of the <see cref="Synapse"/>.
    /// </summary>
    public double? Strength { get; set; }
}