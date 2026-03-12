// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;
using ILGPU;
using ILGPU.Runtime;

namespace Spatial.Compute.Acceleration;

/// <summary>
/// Benchmarks for <see cref="KernelJob"/>.
/// </summary>
[GcForce]
[GcServer]
[MemoryDiagnoser]
public class KernelJobBenchmarks
{
    /// <summary>
    /// The size of the test matrix.
    /// </summary>
    [Params(256, 512)]
    public int MatrixSize { get; set; }

    private float[,] _a = null!;
    private float[,] _b = null!;
    private float[,] _c = null!;

    private Accelerator _accelerator = null!;

    private MemoryBuffer2D<float, Stride2D.DenseX> _ka = null!;
    private MemoryBuffer2D<float, Stride2D.DenseX> _kb = null!;
    private MemoryBuffer2D<float, Stride2D.DenseX> _kc = null!;

    private Computer _computer = null!;

    /// <summary>
    /// Setup the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _a = CreateRandomMatrix(MatrixSize);
        _b = CreateRandomMatrix(MatrixSize);
        _c = new float[MatrixSize, MatrixSize];

        (_computer = new()).Run();

        _accelerator = Job.Accelerator();

        var m  = _a.GetLength(0);
        var ka = _a.GetLength(1);
        var n  = _b.GetLength(1);

        _ka = _accelerator.Allocate2DDenseX<float>(new Index2D(m, ka));
        _kb = _accelerator.Allocate2DDenseX<float>(new Index2D(ka, n));
        _kc = _accelerator.Allocate2DDenseX<float>(new Index2D(m, n));

        _ka.CopyFromCPU(_a);
        _kb.CopyFromCPU(_b);
    }

    /// <summary>
    /// Clean up after running the benchmarks.
    /// </summary>
    [GlobalCleanup]
    public void Cleanup()
    {
        _ka.Dispose();
        _kb.Dispose();
        _kc.Dispose();
        _accelerator.Dispose();
        _computer.Shutdown();
    }

    /// <summary>
    /// Measure kernel matrix multiplication.
    /// </summary>
    [Benchmark]
    public void MatrixMultiplication()
    {
        Job.Kernel(_accelerator, _kc.IntExtent, _ka.View, _kb.View, _kc.View, (index, a, b, c) => {
            var x   = index.X;
            var y   = index.Y;
            var sum = 0F;

            for (var i = 0; i < a.IntExtent.Y; i++)
            {
                sum += a[new Index2D(x, i)] * b[new Index2D(i, y)];
            }

            c[index] = sum;
        }).Wait();

        _kc.CopyToCPU(_c);
    }

    private static float[,] CreateRandomMatrix(int size)
    {
        var matrix = new float[size, size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                matrix[i,j] = Strong.Float();
            }
        }

        return matrix;
    }
}