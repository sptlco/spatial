// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation;

namespace Spatial.Cloud.ECS.Components;

/// <summary>
/// A weighted connection between two neurons.
/// </summary>
/// <param name="From">The pre-synaptic <see cref="Neuron"/>, from which the <see cref="Synapse"/> originates.</param>
/// <param name="To">The post-synaptic <see cref="Neuron"/>, to which the <see cref="Synapse"/> extends.</param>
/// <param name="Strength">The strength of the <see cref="Synapse"/>.</param>
public record struct Synapse(
    uint From,
    uint To,
    double Strength) : IComponent;