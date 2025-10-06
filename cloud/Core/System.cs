// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation;

namespace Spatial;

/// <summary>
/// An object that runs routine code.
/// </summary>
public abstract class System
{
    /// <summary>
    /// Create the <see cref="System"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public virtual void Create(Space space) { }

    /// <summary>
    /// Execute code before updating the <see cref="System"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public virtual void BeforeUpdate(Space space) { }

    /// <summary>
    /// Update the <see cref="System"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> passed since the last run.</param>
    public virtual void Update(Space space, Time delta) { }

    /// <summary>
    /// Execute code after updating the <see cref="System"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public virtual void AfterUpdate(Space space) { }

    /// <summary>
    /// Destroy the <see cref="System"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    public virtual void Destroy(Space space) { }
}