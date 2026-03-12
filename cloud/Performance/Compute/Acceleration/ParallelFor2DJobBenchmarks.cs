// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;

namespace Spatial.Compute.Acceleration;

/// <summary>
/// Benchmarks for <see cref="ParallelFor2DJob"/>.
/// </summary>
[GcForce]
[GcServer]
[MemoryDiagnoser]
public class ParallelFor2DJobBenchmarks
{
    /// <summary>
    /// The size of the test matrix.
    /// </summary>
    [Params(256, 384, 512)]
    public int MatrixSize { get; set; }

    private float[][] _a = null!;
    private float[][] _b = null!;
    private float[][] _c = null!;

    private float[][] _bt = null!;

    private Computer _computer = null!;

    /// <summary>
    /// Setup the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _a  = CreateRandomMatrix(MatrixSize);
        _b  = CreateRandomMatrix(MatrixSize);
        _bt = Transpose(_b, MatrixSize);
        _c  = CreateZeroMatrix(MatrixSize);

        (_computer = new()).Run();
    }

    /// <summary>
    /// Clean up after running the benchmarks.
    /// </summary>
    [GlobalCleanup]
    public void Cleanup()
    {
        _computer.Shutdown();
    }

    /// <summary>
    /// Baseline: single-threaded sequential matrix multiplication.
    /// Measures the raw cost of the work with zero scheduling overhead.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void SequentialParallelFor2D()
    {
        Job.ParallelFor2D(
            MatrixSize,
            MatrixSize,
            options: new JobOptions { BatchStrategy = BatchStrategy.None },
            function: (i, j) =>
            {
                var ai  = _a[i];
                var bti = _bt[j];
                var sum = 0F;

                for (var k = 0; k < MatrixSize; k++)
                {
                    sum += ai[k] * bti[k];
                }

                _c[i][j] = sum;
            }).Wait();
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
                var ai  = _a[i];
                var bti = _bt[j];
                var sum = 0F;

                for (var k = 0; k < MatrixSize; k++)
                {
                    sum += ai[k] * bti[k];
                }

                _c[i][j] = sum;
            }).Wait();
    }

    private static float[][] CreateRandomMatrix(int size)
    {
        var matrix = new float[size][];

        for (var i = 0; i < size; i++)
        {
            matrix[i] = new float[size];

            for (var j = 0; j < size; j++)
            {
                matrix[i][j] = Strong.Float();
            }
        }

        return matrix;
    }

    private static float[][] CreateZeroMatrix(int size)
    {
        var matrix = new float[size][];

        for (var i = 0; i < size; i++)
        {
            matrix[i] = new float[size];
        }

        return matrix;
    }

    private static float[][] Transpose(float[][] m, int size)
    {
        var t = new float[size][];

        for (var i = 0; i < size; i++)
        {
            t[i] = new float[size];

            for (var j = 0; j < size; j++)
            {
                t[i][j] = m[j][i];
            }
        }

        return t;
    }
}