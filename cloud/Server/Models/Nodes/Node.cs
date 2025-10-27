// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Components;
using Spatial.Cloud.Models.Users;
using Spatial.Mathematics;
using Spatial.Persistence;

namespace Spatial.Cloud.Models.Nodes;

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
    /// The <see cref="Systems.IActuator"/> the <see cref="Neuron"/> routes to.
    /// </summary>
    public int Actuator { get; set; }

    /// <summary>
    /// For <see cref="NeuronType.Sensory"/> and <see cref="NeuronType.Motor"/> neurons, the channel the neuron maps to.
    /// </summary>
    public int Channel { get; set; }

    /// <summary>
    /// The precise location of the <see cref="Node"/>.
    /// </summary>
    public Point3D Position { get; set; } = Point3D.Zero;

    /// <summary>
    /// The node's rotation.
    /// </summary>
    public Point3D Rotation { get; set; } = Point3D.Zero;

    /// <summary>
    /// The node's current value.
    /// </summary>
    public double Value { get; set; } = 0.0D;
}