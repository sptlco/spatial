// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Compute.Commands;

/// <summary>
/// A simple <see cref="Job"/> that performs an <see cref="Action"/>.
/// </summary>
public class CommandJob : Job
{
    internal Action _function;

    /// <summary>
    /// Create a new <see cref="CommandJob"/>.
    /// </summary>
    protected CommandJob() { }

    /// <summary>
    /// Create a new <see cref="CommandJob"/>.
    /// </summary>
    /// <param name="function">The <see cref="Action"/> to execute.</param>
    public CommandJob(Action function)
    {
        _function = function;
    }

    /// <summary>
    /// Execute the <see cref="CommandJob"/> function.
    /// </summary>
    public virtual void Execute()
    {
        _function();
    }
}