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
    public string Id { get; init; } = id;

    /// <summary>
    /// The neuron's <see cref="NeuronType"/>.
    /// </summary>
    public NeuronType Type { get; init; } = type;

    /// <summary>
    /// The group the <see cref="Neuron"/> belongs to.
    /// </summary>
    public int Group { get; init; } = group;

    /// <summary>
    /// For <see cref="NeuronType.Sensory"/> and <see cref="NeuronType.Motor"/> neurons, the channel the neuron maps to.
    /// </summary>
    public int Channel { get; init; } = channel;

    /// <summary>
    /// The precise location of the <see cref="Neuron"/>.
    /// </summary>
    public Point3D Position { get; init; } = position;

    /// <summary>
    /// The neuron's current activation level.
    /// </summary>
    public double Value { get; init; } = value;
}