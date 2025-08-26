// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Simulation.Systems;

/// <summary>
/// A <see cref="System{Space}"/> that counts.
/// </summary>
public class Counter : System<Space>
{
    private int _count;

    /// <summary>
    /// The current count.
    /// </summary>
    public int Count => _count;

    public override void BeforeUpdate(Space space)
    {
        _count++;
    }

    public override void Update(Space space, Time delta)
    {
        _count++;
    }

    public override void AfterUpdate(Space space)
    {
        _count++;
    }
}