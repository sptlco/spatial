// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Intelligence.Synapses;

/// <summary>
/// An immutable point-in-time view of a <see cref="Synapse"/>.
/// </summary>
public readonly struct SynapseSnapshot(string id, string from, string to, double strength)
{
    /// <summary>
    /// The synapse's identifier.
    /// </summary>
    public string Id { get; init; } = id;

    /// <summary>
    /// The <see cref="Neurons.Neuron"/> the <see cref="Synapse"/> extends from.
    /// </summary>
    public string From { get; init; } = from;

    /// <summary>
    /// The <see cref="Neurons.Neuron"/> the <see cref="Synapse"/> extends to.
    /// </summary>
    public string To { get; init; } = to;

    /// <summary>
    /// The synapse's current strength.
    /// </summary>
    public double Strength { get; init; } = strength;
}