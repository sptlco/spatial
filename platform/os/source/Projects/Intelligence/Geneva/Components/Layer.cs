// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Geneva.Components;

/// <summary>
/// A specific region of the brain.
/// </summary>
/// <param name="Type">The layer's <see cref="LayerType"/>.</param>
/// <param name="Depth">The layer's depth.</param>
public record struct Layer(LayerType Type, int Depth = 0) : IComponent;

/// <summary>
/// Specifies a region of the brain.
/// </summary>
public enum LayerType
{
    /// <summary>
    /// The area of the brain that processes sensory inputs.
    /// </summary>
    Input,

    /// <summary>
    /// The area of the brain that processes intermediate connections.
    /// </summary>
    Network,

    /// <summary>
    /// The area of the brain that processes motor outputs.
    /// </summary>
    Output
}
