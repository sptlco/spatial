// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Systems;

/// <summary>
/// A sensory-motor I/O interface for physical and virtual systems.
/// </summary>
public interface IActuator
{
    /// <summary>
    /// Extract features from a dataset.
    /// </summary>
    /// <param name="data">Raw data from the input stream.</param>
    /// <returns>An extracted feature vector.</returns>
    double[] Extract(double[] data);

    /// <summary>
    /// Apply a value to the <see cref="IActuator"/>.
    /// </summary>
    /// <param name="channel">An <see cref="IActuator"/> channel.</param>
    /// <param name="value">A numerical value.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    void Apply(int channel, double value, Time delta);
}

/// <summary>
/// Indicates that a class is an <see cref="IActuator"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ActuatorAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="ActuatorAttribute"/>.
    /// </summary>
    /// <param name="name">The actuator's identification number.</param>
    public ActuatorAttribute(int id)
    {
        Id = id;
    }
    
    /// <summary>
    /// The actuator's identification number.
    /// </summary>
    public int Id { get; }
}