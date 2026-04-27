// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Mathematics;

namespace Spatial.Cloud.Data.Intelligence.Neurons;

/// <summary>
/// Configurable options for a <see cref="Neuron"/> update.
/// </summary>
public class UpdateNeuronOptions
{
    /// <summary>
    /// The neuron's <see cref="NeuronType"/>.
    /// </summary>
    public NeuronType? Type { get; set; }

    /// <summary>
    /// The neuron's group identifier.
    /// </summary>
    public int? Group { get; set; }

    /// <summary>
    /// The channel the <see cref="Neuron"/> maps to.
    /// </summary>
    public int? Channel { get; set; }

    /// <summary>
    /// The neuron's current position.
    /// </summary>
    public Point3D? Position { get; set; }
}