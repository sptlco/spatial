// Copyright © Spatial Corporation. All rights reserved.

using Serilog;
using Spatial.Compute.Commands;

namespace Spatial.Compute.Acceleration;

/// <summary>
/// A one-dimensional <see cref="ParallelJob"/>.
/// </summary>
public sealed class ParallelForJob : ParallelJob
{
    private readonly int _batchSize;
    private new readonly Action<int> _function;

    /// <summary>
    /// Create a new <see cref="ParallelForJob"/>.
    /// </summary>
    /// <param name="iterations">The number of iterations to perform.</param>
    /// <param name="batchSize">The number of iterations to perform per job.</param>
    /// <param name="function">A processing function.</param>
    public ParallelForJob(int iterations, int batchSize, Action<int> function) : base(iterations)
    {
        _batchSize = batchSize;
        _function = function;
    }

    /// <summary>
    /// The number of iterations to perform per job.
    /// </summary>
    public int BatchSize => CalculateBatchSize(_iterations, _batchSize, Options.BatchStrategy);

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
            BatchStrategy.Auto => Math.Max(1, iterations / (Environment.ProcessorCount * 8)),
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
}

/// <summary>
/// A batch of iterations of a <see cref="ParallelForJob"/>.
/// </summary>
internal sealed class BatchJob : CommandJob
{
    private ParallelForJob _parent;
    private int _start;
    private int _end;

    /// <summary>
    /// Create a new <see cref="BatchJob"/>.
    /// </summary>
    /// <param name="parent">The job's parent <see cref="ParallelForJob"/>.</param>
    /// <param name="start">The start of the job's iteration range.</param>
    /// <param name="end">The end of the job's iteration range.</param>
    public BatchJob(ParallelForJob parent, int start, int end)
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
    /// The batch's start index.
    /// </summary>
    public int Start => _start;

    /// <summary>
    /// The batch's end index.
    /// </summary>
    public int End => _end;

    /// <summary>
    /// The parent <see cref="ParallelJob"/>.
    /// </summary>
    public ParallelForJob Parent => _parent;

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
}