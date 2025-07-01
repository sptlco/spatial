// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;
using ILGPU;
using ILGPU.Runtime;
using Spatial.Mathematics;

namespace Spatial.Compute.Jobs.Acceleration;

/// <summary>
/// Benchmarks for <see cref="KernelJob"/>.
/// </summary>
[GcForce]
[GcServer]
[MemoryDiagnoser]
public class KernelBenchmarks
{
    /// <summary>
    /// The size of the test matrix.
    /// </summary>
    [Params(256, 512)]
    public int MatrixSize { get; set; }

    private float[,] _a;
    private float[,] _b;
    private float[,] _c;

    private Accelerator _accelerator;

    private MemoryBuffer2D<float, Stride2D.DenseX> _ka;
    private MemoryBuffer2D<float, Stride2D.DenseX> _kb;
    private MemoryBuffer2D<float, Stride2D.DenseX> _kc;

    /// <summary>
    /// Setup the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _a = CreateRandomMatrix(MatrixSize);
        _b = CreateRandomMatrix(MatrixSize);
        _c = new float[MatrixSize, MatrixSize];

        _accelerator = Job.Accelerator();

        var m = _a.GetLength(0);
        var ka = _a.GetLength(1);
        var n = _b.GetLength(1);

        _ka = _accelerator.Allocate2DDenseX<float>(new Index2D(m, ka));
        _kb = _accelerator.Allocate2DDenseX<float>(new Index2D(ka, n));
        _kc = _accelerator.Allocate2DDenseX<float>(new Index2D(m, n));

        _ka.CopyFromCPU(_a);
        _kb.CopyFromCPU(_b);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _ka.Dispose();
        _kb.Dispose();
        _kc.Dispose();

        _accelerator.Dispose();
    }

    /// <summary>
    /// Measure kernel matrix multiplication.
    /// </summary>
    [Benchmark]
    public void MatrixMultiplication()
    {
        Job.Kernel(_accelerator, _kc.IntExtent, _ka.View, _kb.View, _kc.View, (index, a, b, c) => {
            var x = index.X;
            var y = index.Y;
            var sum = 0F;

            for (var i = 0; i < a.IntExtent.Y; i++)
            {
                sum += a[new Index2D(x, i)] * b[new Index2D(i, y)];
            }

            c[index] = sum;
        });

        _kc.CopyToCPU(_c);
    }

    private static float[,] CreateRandomMatrix(int size)
    {
        var matrix = new float[size, size];
        
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
