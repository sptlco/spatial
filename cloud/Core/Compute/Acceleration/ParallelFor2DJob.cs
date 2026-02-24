// Copyright © Spatial Corporation. All rights reserved.

using Serilog;
using Spatial.Compute.Commands;

namespace Spatial.Compute.Acceleration;

/// <summary>
/// A two-dimensional <see cref="ParallelJob"/>.
/// </summary>
public sealed class ParallelFor2DJob : ParallelJob
{
    private readonly int _width, _height;
    private readonly int _batchSizeX, _batchSizeY;
    private new readonly Action<int, int> _function;

    /// <summary>
    /// Create a new <see cref="ParallelFor2DJob"/>.
    /// </summary>
    /// <param name="width">The size of the first dimension.</param>
    /// <param name="height">The size of the second dimension.</param>
    /// <param name="batchSizeX">The number of iterations to perform per job in the first dimension.</param>
    /// <param name="batchSizeY">The number of iterations to perform per job in the second dimension.</param>
    /// <param name="function">A processing function.</param>
    public ParallelFor2DJob(
        int width,
        int height,
        int batchSizeX,
        int batchSizeY,
        Action<int, int> function) : base(width * height)
    {
        _width = width;
        _height = height;
        _batchSizeX = batchSizeX;
        _batchSizeY = batchSizeY;
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
    public int BatchSizeX => ParallelForJob.CalculateBatchSize(_width, _batchSizeX, Options.BatchStrategy);

    /// <summary>
    /// The number of iterations to perform per job in the second dimension.
    /// </summary>
    public int BatchSizeY => ParallelForJob.CalculateBatchSize(_height, _batchSizeY, Options.BatchStrategy);

    /// <summary>
    /// Execute the <see cref="ParallelFor2DJob"/>.
    /// </summary>
    /// <param name="iteration">The current iteration.</param>
    public override void Execute(int iteration)
    {
        _function(iteration % _width, iteration / _width);
    }
}

/// <summary>
/// A batch of iterations of a <see cref="ParallelFor2DJob"/>.
/// </summary>
internal sealed class Batch2DJob : CommandJob
{
    private ParallelFor2DJob _parent;
    private int _startX, _endX;
    private int _startY, _endY;

    /// <summary>
    /// Create a new <see cref="Batch2DJob"/>.
    /// </summary>
    /// <param name="parent">The job's parent <see cref="ParallelFor2DJob"/>.</param>
    /// <param name="startX">The beginning of the iteration range of the first dimension.</param>
    /// <param name="endX">The end of the iteration range of the first dimension.</param>
    /// <param name="startY">The beginning of the iteration range of the second dimension.</param>
    /// <param name="endY">The end of the iteration range of the second dimension.</param>
    public Batch2DJob(ParallelFor2DJob parent, int startX, int endX, int startY, int endY)
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
    /// The batch's starting X index.
    /// </summary>
    public int StartX => _startX;

    /// <summary>
    /// The batch's starting Y index.
    /// </summary>
    public int StartY => _startY;

    /// <summary>
    /// The batch's ending X index.
    /// </summary>
    public int EndX => _endX;

    /// <summary>
    /// The batch's ending Y index.
    /// </summary>
    public int EndY => _endY;

    /// <summary>
    /// The job's parent <see cref="ParallelFor2DJob"/>.
    /// </summary>
    public ParallelFor2DJob Parent => _parent;

    /// <summary>
    /// Execute the <see cref="Batch2DJob"/>.
    /// </summary>
    public override void Execute()
    {
        for (var x = _startX; x < _endX; x++)
        {
            for (var y = _startY; y < _endY; y++)
            {
                try
                {
                    _parent.Execute(y * _parent.Width + x);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Iteration ({IterationX}, {IterationY})/({IterationsX}, {IterationsY}) failed.", x + 1, y + 1, _parent.Width, _parent.Height);
                    throw;
                }
            }
        }
    }
}