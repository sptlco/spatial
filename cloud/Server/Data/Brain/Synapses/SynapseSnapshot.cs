// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Brain.Synapses;

/// <summary>
/// An immutable point-in-time view of a <see cref="Synapse"/>.
/// </summary>
public readonly struct SynapseSnapshot(string id, string from, string to, double strength)
{
    /// <summary>
    /// The synapse's identifier.
    /// </summary>
    public readonly string Id = id;

    /// <summary>
    /// The <see cref="Neurons.Neuron"/> the <see cref="Synapse"/> extends from.
    /// </summary>
    public readonly string From = from;

    /// <summary>
    /// The <see cref="Neurons.Neuron"/> the <see cref="Synapse"/> extends to.
    /// </summary>
    public readonly string To = to;

    /// <summary>
    /// The synapse's current strength.
    /// </summary>
    public readonly double Strength = strength;
}