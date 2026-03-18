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
    /// Execute the <see cref="ParallelForJob"/>.
    /// </summary>
    /// <param name="iteration">The current iteration.</param>
    public override void Execute(int iteration)
    {
        _function(iteration);
    }

    private static int CalculateBatchSize(int iterations, int preference, BatchStrategy strategy = BatchStrategy.Auto)
    {
        return strategy switch {
            BatchStrategy.None => iterations,
            BatchStrategy.Preferred => Math.Max(1, preference),
            BatchStrategy.Auto => Math.Max(1, iterations / Environment.ProcessorCount),
            _ => Math.Max(1, preference)
        };
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
        Reset(parent, start, end);
    }

    /// <summary>
    /// No-op. Batch jobs are not tracked globally.
    /// </summary>
    public override string Id { get; internal set; } = string.Empty;

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

    /// <summary>
    /// Reset the <see cref="BatchJob"/>.
    /// </summary>
    /// <param name="parent">The job's parent.</param>
    /// <param name="start">The job's starting index.</param>
    /// <param name="end">The job's ending index.</param>
    public void Reset(ParallelForJob parent, int start, int end)
    {
        _parent = parent;
        _start = start;
        _end = end;
    }
}