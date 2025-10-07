// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute.Commands;

namespace Spatial.Compute.Acceleration;

/// <summary>
/// A <see cref="Job"/> whose iterations are executed concurrently.
/// </summary>
public abstract class ParallelJob : CommandJob
{
    /// <summary>
    /// The job's iteration count.
    /// </summary>
    protected int _iterations;

    /// <summary>
    /// The job's completed iteration count.
    /// </summary>
    protected int _completed;

    /// <summary>
    /// Create a new <see cref="ParallelJob"/>.
    /// </summary>
    /// <param name="iterations">The number of iterations to perform.</param>
    public ParallelJob(int iterations)
    {
        _iterations = iterations;
    }

    /// <summary>
    /// The number of iterations to execute the <see cref="ParallelJob"/> for.
    /// </summary>
    public int Iterations => _iterations;

    /// <summary>
    /// Get whether or not the <see cref="ParallelJob"/> is complete.
    /// </summary>
    /// <param name="iterations">The number of completed iterations.</param>
    /// <returns>Whether or not the <see cref="ParallelJob"/> is complete.</returns>
    public bool Complete(int iterations)
    {
        if (Interlocked.Add(ref _completed, iterations) >= _iterations)
        {
            Status = JobStatus.Complete;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Execute the <see cref="ParallelJob"/>.
    /// </summary>
    /// <param name="iteration">The current iteration.</param>
    public abstract void Execute(int iteration);
}

/// <summary>
/// Indicates the batching strategy for a <see cref="ParallelJob"/>.
/// Default is <see cref="Auto"/>.
/// </summary>
public enum BatchStrategy
{
    /// <summary>
    /// The job will run in a single batch.
    /// </summary>
    None,

    /// <summary>
    /// The system will automatically determine the optimal batch size.
    /// </summary>
    Auto,

    /// <summary>
    /// The system will use the preferred batch size.
    /// </summary>
    Preferred
}