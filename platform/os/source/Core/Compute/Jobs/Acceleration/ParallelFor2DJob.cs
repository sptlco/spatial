// Copyright © Spatial. All rights reserved.

using Spatial.Compute.Jobs.Commands;
using System.Collections.Concurrent;

namespace Spatial.Compute.Jobs.Acceleration;

/// <summary>
/// A two-dimensional <see cref="ParallelJob"/>.
/// </summary>
internal sealed class ParallelFor2DJob : ParallelJob
{
    private static readonly ConcurrentBag<ParallelFor2DJob> _jobs = [];

    private int _width, _height;
    private int _batchSizeX, _batchSizeY;
    private Action<int, int> _function;

    private ParallelFor2DJob(
        int width,
        int height,
        int batchSizeX,
        int batchSizeY,
        Action<int, int> function,
        BatchStrategy batchStrategy = BatchStrategy.Auto) : base(width * height)
    {
        _width = width;
        _height = height;
        _batchSizeX = ParallelForJob.CalculateBatchSize(_width, batchSizeX, batchStrategy);
        _batchSizeY = ParallelForJob.CalculateBatchSize(_height, batchSizeY, batchStrategy);
        _function = function;
    }

    /// <summary>
    /// The size of the first dimension in the <see cref="ParallelFor2DJob"/>.
    /// </summary>
    public int Width => _width;

    /// <summary>
    /// The size of the second dimension in the <see cref="ParallelFor2DJob"/>.
    /// </summary>
    public int Height => _height;

    /// <summary>
    /// The number of iterations to perform per job in the first dimension.
    /// </summary>
    public int BatchSizeX => _batchSizeX;

    /// <summary>
    /// The number of iterations to perform per job in the second dimension.
    /// </summary>
    public int BatchSizeY => _batchSizeY;

    /// <summary>
    /// Create a new <see cref="ParallelFor2DJob"/>.
    /// </summary>
    /// <param name="width">The size of the first dimension.</param>
    /// <param name="height">The size of the second dimension.</param>
    /// <param name="batchSizeX">The number of iterations to perform per job in the first dimension.</param>
    /// <param name="batchSizeY">The number of iterations to perform per job in the second dimension.</param>
    /// <param name="function">A processing function.</param>
    /// <param name="batchStrategy">The job's <see cref="BatchStrategy"/>.</param>
    /// <returns>A <see cref="ParallelFor2DJob"/>.</returns>
    public static ParallelFor2DJob Create(int width, int height, int batchSizeX, int batchSizeY, Action<int, int> function, BatchStrategy batchStrategy = BatchStrategy.Auto)
    {
        if (_jobs.TryTake(out var job))
        {
            job._iterations = width * height;
            job._completed = 0;
            job._width = width;
            job._height = height;
            job._batchSizeX = ParallelForJob.CalculateBatchSize(job._width, batchSizeX, batchStrategy);
            job._batchSizeY = ParallelForJob.CalculateBatchSize(job._height, batchSizeY, batchStrategy);
            job._function = function;

            return job;
        }

        return new(width, height, batchSizeX, batchSizeY, function, batchStrategy);
    }

    /// <summary>
    /// Execute the <see cref="ParallelFor2DJob"/>.
    /// </summary>
    /// <param name="iteration">The current iteration.</param>
    public override void Execute(int iteration)
    {
        _function(iteration % _width, iteration / _width);
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

/// <summary>
/// A batch of iterations of a <see cref="ParallelFor2DJob"/>.
/// </summary>
internal sealed class Batch2DJob : CommandJob
{
    private static readonly ConcurrentBag<Batch2DJob> _jobs = [];

    private ParallelFor2DJob _parent;
    private int _startX, _endX;
    private int _startY, _endY;

    private Batch2DJob(ParallelFor2DJob parent, int startX, int endX, int startY, int endY)
    {
        _parent = parent;
        _startX = startX;
        _endX = endX;
        _startY = startY;
        _endY = endY;
    }

    /// <summary>
    /// The number of iterations performed by the <see cref="Batch2DJob"/>.
    /// </summary>
    public int Size => (_endX - _startX) * (_endY - _startY);

    /// <summary>
    /// The job's parent <see cref="ParallelFor2DJob"/>.
    /// </summary>
    public ParallelFor2DJob Parent => _parent;

    /// <summary>
    /// Create a new <see cref="Batch2DJob"/>.
    /// </summary>
    /// <param name="parent">The job's parent <see cref="ParallelFor2DJob"/>.</param>
    /// <param name="startX">The beginning of the iteration range of the first dimension.</param>
    /// <param name="endX">The end of the iteration range of the first dimension.</param>
    /// <param name="startY">The beginning of the iteration range of the second dimension.</param>
    /// <param name="endY">The end of the iteration range of the second dimension.</param>
    /// <returns>A <see cref="Batch2DJob"/>.</returns>
    public static Batch2DJob Create(ParallelFor2DJob parent, int startX, int endX, int startY, int endY)
    {
        if (_jobs.TryTake(out var job))
        {
            job._parent = parent;
            job._startX = startX;
            job._endX = endX;
            job._startY = startY;
            job._endY = endY;

            return job;
        }

        return new(parent, startX, endX, startY, endY);
    }

    /// <summary>
    /// Execute the <see cref="Batch2DJob"/>.
    /// </summary>
    public override void Execute()
    {
        for (var x = _startX; x < _endX; x++)
        {
            for (var y = _startY; y < _endY; y++)
            {
                _parent.Execute(y * _parent.Width + x);
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