// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Geneva.Components;

/// <summary>
/// A connection between two neurons.
/// </summary>
/// <param name="Source">The pre-synaptic <see cref="Neuron"/>.</param>
/// <param name="Destination">The post-synaptic <see cref="Neuron"/>.</param>
/// <param name="Strength">The strength of the connection.</param>
public record struct Synapse(
    uint Source,
    uint Destination,
    double Strength) : IComponent;