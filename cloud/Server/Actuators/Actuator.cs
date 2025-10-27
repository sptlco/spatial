// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Extensions;
using System.Reflection;

namespace Spatial.Cloud.Actuators;

/// <summary>
/// Specifies that a class represents an actuator.
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

    /// <summary>
    /// The name of the actuator.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A message describing what the actuator does.
    /// </summary>
    public string Description { get; set; }
}

/// <summary>
/// A value produced by neural activation.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class NeuralOutputAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="NeuralOutputAttribute"/>.
    /// </summary>
    /// <param name="channel">The output's channel number.</param>
    /// <param name="value">The output's default value.</param>
    public NeuralOutputAttribute(int channel = 0, double value = 0.0D)
    {
        Channel = channel;
        Default = value;
    }

    /// <summary>
    /// The output's channel number.
    /// </summary>
    public int Channel { get; set; }

    /// <summary>
    /// The output's default value.
    /// </summary>
    public double Default { get; set; }

    /// <summary>
    /// The name of the output.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A message describing the output.
    /// </summary>
    public string Description { get; set; }
}

/// <summary>
/// An adjustable <see cref="Control"/> property.
/// </summary>
public class Property
{
    /// <summary>
    /// Create a new <see cref="Property"/>.
    /// </summary>
    /// <param name="value">The property's initial value.</param>
    public Property(double value = 0.0D)
    {
        Current = Target = value;
    }

    /// <summary>
    /// The property's current value.
    /// </summary>
    public double Current { get; set; }

    /// <summary>
    /// The property's target value.
    /// </summary>
    public double Target { get; set; }
}

/// <summary>
/// A sensory-motor I/O interface for physical and virtual systems.
/// </summary>
public abstract class Control
{
    private readonly Dictionary<int, Property> _motors;

    /// <summary>
    /// Create a new <see cref="Control"/>.
    /// </summary>
    public Control()
    {
        var motors = GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.PropertyType == typeof(Property) && property.GetCustomAttribute<NeuralOutputAttribute>() != default)
            .Select(property => (Property: property, Attribute: property.GetCustomAttribute<NeuralOutputAttribute>()!));

        motors.ForEach(motor => motor.Property.SetValue(this, new Property(motor.Attribute.Default)));

        _motors = motors.ToDictionary(
            keySelector: motor => motor.Attribute.Channel,
            elementSelector: motor => (Property) motor.Property.GetValue(this)!);
    }

    /// <summary>
    /// Extract features from a dataset.
    /// </summary>
    /// <param name="data">Raw data from the input stream.</param>
    /// <returns>An extracted feature vector.</returns>
    public abstract double[] Extract(double[] data);

    /// <summary>
    /// Apply a value to a <see cref="Property"/>.
    /// </summary>
    /// <param name="channel">A <see cref="Property"/> channel.</param>
    /// <param name="value">A <see cref="Property"/> value.</param>
    public void Apply(int channel, double value)
    {
        if (_motors.TryGetValue(channel, out var property))
        {
            property.Target = value;
        }
    }

    /// <summary>
    /// Update the <see cref="Control"/>.
    /// </summary>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public virtual void Update(Time delta) { }
}