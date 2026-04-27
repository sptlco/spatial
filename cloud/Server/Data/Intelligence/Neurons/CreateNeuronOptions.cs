// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Mathematics;

namespace Spatial.Cloud.Data.Intelligence.Neurons;

/// <summary>
/// Configurable options for a new <see cref="Neuron"/>.
/// </summary>
public class CreateNeuronOptions : CreateResourceOptions
{
    /// <summary>
    /// The neuron's <see cref="NeuronType"/>.
    /// </summary>
    public NeuronType Type { get; set; }

    /// <summary>
    /// The neuron's group identification number.
    /// </summary>
    public int Group { get; set; }

    /// <summary>
    /// The channel the <see cref="Neuron"/> maps to.
    /// </summary>
    public int Channel { get; set; }

    /// <summary>
    /// The neuron's current position.
    /// </summary>
    public Point3D Position { get; set; } = Point3D.Zero;

    /// <summary>
    /// The neuron's current activation level.
    /// </summary>
    public double Value { get; set; }
}