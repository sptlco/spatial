// Copyright © Spatial. All rights reserved.

using Ignite.Models.Objects;

namespace Ignite.Contracts;

/// <summary>
/// Indicates the current state of an <see cref="ObjectRef"/>.
/// </summary>
public enum ObjectMode
{
    Default = 1,
    // ...
    Dead = 3,
    Resting = 4,
    Vending = 5,
    Riding = 6
}