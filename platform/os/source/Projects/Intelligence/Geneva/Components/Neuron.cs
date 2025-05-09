// Copyright © Spatial. All rights reserved.

using Geneva.Models;
using Spatial.Simulation;

namespace Geneva.Components;

/// <summary>
/// An autonomous component of the <see cref="Schematic"/>.
/// </summary>
/// <param name="Charge">The neuron's current charge in millivolts (mV).</param>
/// <param name="State">The neuron's current <see cref="Components.State"/>.</param>
public record struct Neuron(
    double Charge,
    State State = State.Resting) : IComponent;

/// <summary>
/// The current state of a <see cref="Neuron"/>.
/// </summary>
public enum State
{
    /// <summary>
    /// The <see cref="Neuron"/> is at rest.
    /// </summary>
    Resting,

    /// <summary>
    /// The <see cref="Neuron"/> is rising to its action potential.
    /// </summary>
    Depolarizing,

    /// <summary>
    /// The <see cref="Neuron"/> is falling from its action potential.
    /// </summary>
    Repolarizing,

    /// <summary>
    /// The <see cref="Neuron"/> is returning to rest.
    /// </summary>
    Hyperpolarizing
}