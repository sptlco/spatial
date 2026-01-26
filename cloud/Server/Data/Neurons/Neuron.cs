// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Mathematics;
using Spatial.Persistence;

namespace Spatial.Cloud.Data.Neurons;

/// <summary>
/// ...
/// </summary>
[Collection("neurons")]
public class Neuron : Resource
{
    /// <summary>
    /// The node's <see cref="NeuronType"/>.
    /// </summary>
    public NeuronType Type { get; set; }

    /// <summary>
    /// The <see cref="ECS.Systems.Transducer"/> the <see cref="Neuron"/> belongs to.
    /// </summary>
    public int Group { get; set; }

    /// <summary>
    /// For <see cref="NeuronType.Sensory"/> and <see cref="NeuronType.Motor"/> neurons, the channel the neuron maps to.
    /// </summary>
    public int Channel { get; set; }

    /// <summary>
    /// The precise location of the <see cref="Neuron"/>.
    /// </summary>
    public Point3D Position { get; set; } = Point3D.Zero;

    /// <summary>
    /// The node's current value.
    /// </summary>
    public double Value { get; set; } = 0.0D;
}