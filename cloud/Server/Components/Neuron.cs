// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation;

namespace Spatial.Cloud.Components;

/// <summary>
/// ...
/// </summary>
public record struct Neuron(NeuronType Type, int Group, int Channel, double Value) : IComponent;

/// <summary>
/// Specifies the function of a <see cref="Neuron"/>.
/// </summary>
public enum NeuronType
{
    /// <summary>
    /// A <see cref="Neuron"/> that receives and feeds the current sensory information into the network.
    /// </summary>
    Sensory,

    /// <summary>
    /// A <see cref="Neuron"/> whose internal state is time-dependent.
    /// </summary>
    Temporal,

    /// <summary>
    /// A <see cref="Neuron"/> that converts complex temporal neural dynamics into control signals.
    /// </summary>
    Command,

    /// <summary>
    /// A <see cref="Neuron"/> that directly controls the system's actuator.
    /// </summary>
    Motor
}