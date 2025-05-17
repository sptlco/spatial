// Copyright © Spatial. All rights reserved.

namespace Ignite.Models;

/// <summary>
/// Indicates the current state of an <see cref="Object"/>.
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