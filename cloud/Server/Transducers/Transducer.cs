// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Extensions;
using System.Reflection;

namespace Spatial.Cloud.Transducers;

/// <summary>
/// A device that converts neural input signals to output values.
/// </summary>
public abstract class Transducer
{
    private readonly Dictionary<int, Property> _outputs;

    /// <summary>
    /// Create a new <see cref="Transducer"/>.
    /// </summary>
    public Transducer()
    {
        var outputs = GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.PropertyType == typeof(Property) && property.GetCustomAttribute<OutputAttribute>() != default)
            .Select(property => (Property: property, Attribute: property.GetCustomAttribute<OutputAttribute>()!));

        outputs.ForEach(output => output.Property.SetValue(this, new Property(output.Attribute.Default)));

        _outputs = outputs.ToDictionary(
            keySelector: output => output.Attribute.Channel,
            elementSelector: output => (Property) output.Property.GetValue(this)!);
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
        if (_outputs.TryGetValue(channel, out var property))
        {
            property.Target = value;
        }
    }

    /// <summary>
    /// Update the <see cref="Transducer"/>.
    /// </summary>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public virtual void Update(Time delta) { }
}

/// <summary>
/// A group of neurons.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ModuleAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="ModuleAttribute"/>.
    /// </summary>
    /// <param name="name">The module's identification number.</param>
    public ModuleAttribute(int id)
    {
        Id = id;
    }

    /// <summary>
    /// The module's identification number.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// The name of the module.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A message describing what the module does.
    /// </summary>
    public string Description { get; set; }
}

/// <summary>
/// A value produced by neural activation.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class OutputAttribute : Attribute
{
    /// <summary>
    /// Create a new <see cref="OutputAttribute"/>.
    /// </summary>
    /// <param name="channel">The output's channel number.</param>
    /// <param name="value">The output's default value.</param>
    public OutputAttribute(int channel = 0, double value = 0.0D)
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
/// An adjustable <see cref="Transducer"/> property.
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