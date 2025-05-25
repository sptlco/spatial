// Copyright © Spatial. All rights reserved.

using Serilog;
using Spatial.Compute.Jobs.Commands;
using System.Collections.Concurrent;

namespace Spatial.Compute.Jobs.Acceleration;

/// <summary>
/// A one-dimensional <see cref="ParallelJob"/>.
/// </summary>
internal sealed class ParallelForJob : ParallelJob
{
    private static readonly ConcurrentBag<ParallelForJob> _jobs = [];

    private int _batchSize;
    private Action<int> _function;

    private ParallelForJob(int iterations, int batchSize, Action<int> function, BatchStrategy batchStrategy = BatchStrategy.Auto) : base(iterations)
    {
        _batchSize = CalculateBatchSize(iterations, batchSize, batchStrategy);
        _function = function;
    }

    /// <summary>
    /// The number of iterations to perform per job.
    /// </summary>
    public int BatchSize => _batchSize;

    /// <summary>
    /// Create a new <see cref="ParallelForJob"/>.
    /// </summary>
    /// <param name="iterations">The number of iterations to perform.</param>
    /// <param name="batchSize">The number of iterations to perform per job.</param>
    /// <param name="function">A processing function.</param>
    /// <param name="batchStrategy">The job's <see cref="BatchStrategy"/>.</param>
    /// <returns>A <see cref="ParallelForJob"/>.</returns>
    public static ParallelForJob Create(int iterations, int batchSize, Action<int> function, BatchStrategy batchStrategy = BatchStrategy.Auto)
    {
        // if (_jobs.TryTake(out var job))
        // {
        //     job.Reset();

        //     job._iterations = iterations;
        //     job._completed = 0;
        //     job._batchSize = CalculateBatchSize(iterations, batchSize, batchStrategy);
        //     job._function = function;

        //     return job;
        // }

        return new(iterations, batchSize, function, batchStrategy);
    }

    /// <summary>
    /// Calculate the batch size for a <see cref="ParallelForJob"/>.
    /// </summary>
    /// <param name="iterations">The number of iterations to perform.</param>
    /// <param name="preference">The desired batch size.</param>
    /// <param name="strategy">The job's <see cref="BatchStrategy"/>.</param>
    /// <returns>The calculated batch size.</returns>
    public static int CalculateBatchSize(int iterations, int preference, BatchStrategy strategy = BatchStrategy.Auto)
    {
        return strategy switch
        {
            BatchStrategy.None => iterations,
            BatchStrategy.Preferred => Math.Min(preference, iterations),
            BatchStrategy.Auto => Math.Max(1, iterations / (System.Environment.ProcessorCount * 8)),
            _ => throw new ArgumentException("Invalid batch strategy. Cannot determine batch size."),
        };
    }

    /// <summary>
    /// Execute the <see cref="ParallelForJob"/>.
    /// </summary>
    /// <param name="iteration">The current iteration.</param>
    public override void Execute(int iteration)
    {
        _function(iteration);
    }

    /// <summary>
    /// Dispose of the <see cref="Job"/>.
    /// </summary>
    public override void Dispose()
    {
        // if (_jobs.Count < Constants.MaxPoolSize)
        // {
        //     _jobs.Add(this);
        // }
    }
}

/// <summary>
/// A batch of iterations of a <see cref="ParallelForJob"/>.
/// </summary>
internal sealed class BatchJob : CommandJob
{
    private static readonly ConcurrentBag<BatchJob> _jobs = [];

    private ParallelForJob _parent;
    private int _start;
    private int _end;

    private BatchJob(ParallelForJob parent, int start, int end)
    {
        _parent = parent;
        _start = start;
        _end = end;
    }

    /// <summary>
    /// The size of the <see cref="BatchJob"/>.
    /// </summary>
    public int Size => _end - _start;

    /// <summary>
    /// The parent <see cref="ParallelJob"/>.
    /// </summary>
    public ParallelForJob Parent => _parent;

    /// <summary>
    /// Create a new <see cref="BatchJob"/>.
    /// </summary>
    /// <param name="parent">The job's parent <see cref="ParallelForJob"/>.</param>
    /// <param name="start">The start of the job's iteration range.</param>
    /// <param name="end">The end of the job's iteration range.</param>
    /// <returns>A <see cref="BatchJob"/>.</returns>
    public static BatchJob Create(ParallelForJob parent, int start, int end)
    {
        if (_jobs.TryTake(out var job))
        {
            job.Reset();

            job._parent = parent;
            job._start = start;
            job._end = end;

            return job;
        }

        return new(parent, start, end);
    }

    /// <summary>
    /// Execute the <see cref="BatchJob"/>.
    /// </summary>
    public override void Execute()
    {
        for (var i = _start; i < _end; i++)
        {
            try
            {
                _parent.Execute(i);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Iteration {Iteration}/{Iterations} failed.", i + 1, _parent.Iterations);
                throw;
            }
        }
    }

    /// <summary>
    /// Dispose of the <see cref="Job"/>.
    /// </summary>
    public override void Dispose()
    {
        if (_jobs.Count < Constants.MaxPoolSize)
        {
            _jobs.Add(this);
        }
    }
}