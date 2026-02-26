// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;

namespace Spatial.Compute.Acceleration;

/// <summary>
/// Benchmarks for <see cref="ParallelFor2DJob"/>.
/// </summary>
[GcForce]
[GcServer]
[MemoryDiagnoser]
public class JobParallelFor2DBenchmarks
{
    /// <summary>
    /// The size of the test matrix.
    /// </summary>
    [Params(256, 512)]
    public int MatrixSize { get; set; }

    private float[,] _a = null!;
    private float[,] _b = null!;
    private float[,] _c = null!;

    /// <summary>
    /// Setup the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _a = CreateRandomMatrix(MatrixSize);
        _b = CreateRandomMatrix(MatrixSize);
        _c = new float[MatrixSize, MatrixSize];
    }

    /// <summary>
    /// Measure <see cref="Job.ParallelFor2D"/>.
    /// </summary>
    [Benchmark]
    public void ParallelFor2D()
    {
        Job.ParallelFor2D(
            MatrixSize, 
            MatrixSize, 
            options: new JobOptions { BatchStrategy = BatchStrategy.None },
            function: (i, j) =>
            {
                var sum = 0F;

                for (int k = 0; k < MatrixSize; k++)
                {
                    sum += _a[i, k] * _b[k, j];
                }

                _c[i, j] = sum;
            });
    }

    /// <summary>
    /// Measure <see cref="Job.ParallelFor2D"/> with automatic batching.
    /// </summary>
    [Benchmark]
    public void BatchParallelFor2D()
    {
        Job.ParallelFor2D(
            MatrixSize, 
            MatrixSize, 
            function: (i, j) =>
            {
                var sum = 0F;

                for (int k = 0; k < MatrixSize; k++)
                {
                    sum += _a[i, k] * _b[k, j];
                }

                _c[i, j] = sum;
            });
    }

    private static float[,] CreateRandomMatrix(int size)
    {
        var matrix = new float[size, size];
        var random = new Random();
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                matrix[i, j] = Strong.Float();
            }
        }
        
        return matrix;
    }
}