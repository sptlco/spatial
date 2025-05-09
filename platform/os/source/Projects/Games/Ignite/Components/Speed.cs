// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;

namespace Ignite.Components;

/// <summary>
/// The speed of a moving object.
/// </summary>
/// <param name="Walking">The object's walking speed.</param>
/// <param name="Running">The object's running speed.</param>
/// <param name="Rotation">The object's rotation speed.</param>
/// <param name="Attacking">The object's attacking speed.</param>
public record struct Speed(
    float Walking = 0.0F,
    float Running = 0.0F,
    float Rotation = 0.0F,
    float Attacking = 0.0F) : IComponent;