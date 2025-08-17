// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Simulation;

/// <summary>
/// Marks a <see cref="System"/> used by the <see cref="Application"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DependencyAttribute : Attribute
{
    private readonly int _layer;

    /// <summary>
    /// Create a new <see cref="DependencyAttribute"/>.
    /// </summary>
    /// <param name="layer">The system's execution layer.</param>
    public DependencyAttribute(int layer)
    {
        _layer = layer;
    }

    /// <summary>
    /// The system's execution layer.
    /// </summary>
    public int Layer => _layer;
}