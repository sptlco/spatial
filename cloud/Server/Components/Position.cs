// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation;

namespace Spatial.Cloud.Components;

/// <summary>
/// The 3-dimensional position of an <see cref="Entity"/>.
/// </summary>
/// <param name="X">The position of the <see cref="Entity"/> on the X-axis.</param>
/// <param name="Y">The position of the <see cref="Entity"/> on the Y-axis.</param>
/// <param name="Z">The position of the <see cref="Entity"/> on the Z-axis.</param>
public record struct Position(
    double X,
    double Y,
    double Z) : IComponent;