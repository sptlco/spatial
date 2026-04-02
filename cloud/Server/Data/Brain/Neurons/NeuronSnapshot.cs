// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Mathematics;

namespace Spatial.Cloud.Data.Brain.Neurons;

/// <summary>
/// An immutable point-in-time view of a <see cref="Neuron"/>.
/// </summary>
public readonly struct NeuronSnapshot(string id, NeuronType type, int group, int channel, Point3D position, double value)
{
    /// <summary>
    /// The neuron's identifier.
    /// </summary>
    public readonly string Id = id;

    /// <summary>
    /// The neuron's <see cref="NeuronType"/>.
    /// </summary>
    public readonly NeuronType Type = type;

    /// <summary>
    /// The group the <see cref="Neuron"/> belongs to.
    /// </summary>
    public readonly int Group = group;

    /// <summary>
    /// For <see cref="NeuronType.Sensory"/> and <see cref="NeuronType.Motor"/> neurons, the channel the neuron maps to.
    /// </summary>
    public readonly int Channel = channel;

    /// <summary>
    /// The precise location of the <see cref="Neuron"/>.
    /// </summary>
    public readonly Point3D Position = position;

    /// <summary>
    /// The neuron's current activation level.
    /// </summary>
    public readonly double Value = value;
}