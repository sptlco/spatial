// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Components;
using Spatial.Mathematics;
using Spatial.Persistence;

namespace Spatial.Cloud.Models.Brain;

/// <summary>
/// ...
/// </summary>
[Collection("nodes")]
public class Node : Record
{
    /// <summary>
    /// The <see cref="User"/> that created the <see cref="Node"/>.
    /// </summary>
    public string Creator { get; set; }

    /// <summary>
    /// The node's <see cref="NeuronType"/>.
    /// </summary>
    public NeuronType Type { get; set; }

    /// <summary>
    /// The <see cref="Baymax.Transducer"/> the <see cref="Neuron"/> belongs to.
    /// </summary>
    public int Group { get; set; }

    /// <summary>
    /// For <see cref="NeuronType.Sensory"/> and <see cref="NeuronType.Motor"/> neurons, the channel the neuron maps to.
    /// </summary>
    public int Channel { get; set; }

    /// <summary>
    /// The precise location of the <see cref="Node"/>.
    /// </summary>
    public Point3D Position { get; set; } = Point3D.Zero;

    /// <summary>
    /// The node's current value.
    /// </summary>
    public double Value { get; set; } = 0.0D;
}