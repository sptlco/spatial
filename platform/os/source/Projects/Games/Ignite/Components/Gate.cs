// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// An object that teleports objects from one <see cref="Models.Map"/> to another.
/// </summary>
/// <param name="Map">The map the <see cref="Gate"/> links to.</param>
/// <param name="Id">The map's identification number.</param>
/// <param name="X">The X-coordinate the <see cref="Gate"/> leads to.</param>
/// <param name="Y">The Y-coordinate the <see cref="Gate"/> leads to.</param>
/// <param name="R">The rotation the <see cref="Gate"/> leads to.</param>
public record struct Gate(
    byte Map,
    int Id,
    float X,
    float Y,
    float R) : IComponent;