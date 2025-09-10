// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Simulation;

/// <summary>
/// An object that runs routine code.
/// </summary>
public abstract class System<T>
{
    /// <summary>
    /// Create the <see cref="System{T}"/>.
    /// </summary>
    /// <param name="arg">An argument of type <typeparamref name="T"/>.</param>
    public virtual void Create(T arg) { }

    /// <summary>
    /// Execute code before updating the <see cref="System{T}"/>.
    /// </summary>
    /// <param name="arg">An argument of type <typeparamref name="T"/>.</param>
    public virtual void BeforeUpdate(T arg) { }

    /// <summary>
    /// Update the <see cref="System{T}"/>.
    /// </summary>
    /// <param name="arg">An argument of type <typeparamref name="T"/>.</param>
    /// <param name="delta"><see cref="Time"/> passed since the last run.</param>
    public virtual void Update(T arg, Time delta) { }

    /// <summary>
    /// Execute code after updating the <see cref="System{T}"/>.
    /// </summary>
    /// <param name="arg">An argument of type <typeparamref name="T"/>.</param>
    public virtual void AfterUpdate(T arg) { }

    /// <summary>
    /// Destroy the <see cref="System{T}"/>.
    /// </summary>
    /// <param name="arg">An argument of type <typeparamref name="T"/>.</param>
    public virtual void Destroy(T arg) { }
}