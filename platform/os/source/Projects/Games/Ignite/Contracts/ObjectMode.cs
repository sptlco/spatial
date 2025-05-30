// Copyright © Spatial. All rights reserved.

namespace Ignite.Contracts;

/// <summary>
/// Indicates the current state of an object.
/// </summary>
public enum ObjectMode
{
    Default = 1,
    Combat = 2,
    Dead = 3,
    Resting = 4,
    Vending = 5,
    Riding = 6,
    Quitting = 7
}