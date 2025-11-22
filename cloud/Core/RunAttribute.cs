// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// Automatically run a <see cref="System"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RunAttribute : Attribute
{
    private readonly int _order;

    /// <summary>
    /// Create a new <see cref="RunAttribute"/>.
    /// </summary>
    /// <param name="order">The order in which to run the <see cref="System"/>.</param>
    public RunAttribute(int order = 0)
    {
        _order = order;
    }

    /// <summary>
    /// The order in which to run the <see cref="System"/>.
    /// </summary>
    public int Layer => _order;
}