// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Systems;

/// <summary>
/// Something that does a thing.
/// </summary>
public interface IActuator
{
    /// <summary>
    /// Route a value to the <see cref="IActuator"/>.
    /// </summary>
    /// <param name="value">A numerical value.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    void Route(int channel, double value, Time delta);
}

/// <summary>
/// Establishes an <see cref="IActuator"/> channel.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ActuatorAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="ActuatorAttribute"/>.
    /// </summary>
    /// <param name="name">The actuator's name.</param>
    public ActuatorAttribute(string name)
    {
        Name = name;
    }
    
    /// <summary>
    /// The actuator's name.
    /// </summary>
    public string Name { get; }
}