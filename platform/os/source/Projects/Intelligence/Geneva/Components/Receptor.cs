// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Geneva.Components;

/// <summary>
/// A <see cref="Neuron"/> that responds to stimuli.
/// </summary>
/// <param name="Type">The receptor's <see cref="ReceptorType"/>.</param>
/// <param name="Channel">A unique channel identification number.</param>
public record struct Receptor(ReceptorType Type, int Channel) : IComponent;

/// <summary>
/// Specifies the type of a <see cref="Receptor"/>.
/// </summary>
public enum ReceptorType
{
    /// <summary>
    /// The receptor is a sensor.
    /// </summary>
    Sensor,

    /// <summary>
    /// The receptor is a motor.
    /// </summary>
    Motor
}
