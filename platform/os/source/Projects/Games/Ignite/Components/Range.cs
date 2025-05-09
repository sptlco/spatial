// Copyright © Spatial. All rights reserved.

namespace Ignite.Components;

/// <summary>
/// A range of values.
/// </summary>
/// <param name="Minimum">The inclusive minimum value of the <see cref="Range"/>.</param>
/// <param name="Maximum">The inclusive maximum value of the <see cref="Range"/>.</param>
public record struct Range(float Minimum = 0.0F, float Maximum = 0.0F);