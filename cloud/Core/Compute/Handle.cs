// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Compute;

/// <summary>
/// A thread-safe compute handle.
/// </summary>
public sealed class Handle : IDisposable
{
    private CountdownEvent _counter;

    private Handle(int dependencies)
    {
        _counter = new(dependencies);
    }

    /// <summary>
    /// Create a new <see cref="Handle"/>.
    /// </summary>
    /// <param name="dependencies">The handle's dependency count.</param>
    /// <returns>A <see cref="Handle"/>.</returns>
    internal static Handle Create(int dependencies)
    {
        return new(dependencies);
    }

    /// <summary>
    /// Block the calling thread until all dependencies have been executed.
    /// </summary>
    public void Wait()
    {
        _counter.Wait();
    }

    /// <summary>
    /// Signal the execution of a dependency.
    /// </summary>
    internal void Signal()
    {
        _counter.Signal();
    }

    /// <summary>
    /// Dispose of the <see cref="Handle"/>.
    /// </summary>
    public void Dispose()
    {
        _counter.Dispose();
        _counter = null!;
    }
}