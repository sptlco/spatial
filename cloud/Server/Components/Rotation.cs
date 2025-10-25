// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation;

namespace Spatial.Cloud.Components;

/// <summary>
/// The 3-dimensional rotation of an <see cref="Entity"/>.
/// </summary>
/// <param name="X">The degree to which the <see cref="Entity"/> has been rotated along the X-axis.</param>
/// <param name="Y">The degree to which the <see cref="Entity"/> has been rotated along the Y-axis.</param>
/// <param name="Z">The degree to which the <see cref="Entity"/> has been rotated along the Z-axis.</param>
public record struct Rotation(
    double X,
    double Y,
    double Z) : IComponent;