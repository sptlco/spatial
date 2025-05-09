// Copyright © Spatial. All rights reserved.

namespace Spatial.Compute.Jobs.Commands;

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
    private CommandJob(Action function)
    {
        _function = function;
    }

    /// <summary>
    /// Create a new <see cref="CommandJob"/>.
    /// </summary>
    /// <param name="function">The <see cref="Action"/> to perform.</param>
    /// <returns>A <see cref="CommandJob"/>.</returns>
    public static CommandJob Create(Action function)
    {
        return new(function);
    }

    /// <summary>
    /// Create a new <see cref="CommandJob"/>.
    /// </summary>
    /// <param name="function">The <see cref="Action"/> to perform.</param>
    public static implicit operator CommandJob(Action function) => new(function);

    /// <summary>
    /// Execute the <see cref="CommandJob"/> function.
    /// </summary>
    public virtual void Execute()
    {
        _function();
    }
}