// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;

namespace Spatial.Compute.Acceleration;

/// <summary>
/// Benchmarks for <see cref="ParallelForJob"/>.
/// </summary>
[GcForce]
[GcServer]
[MemoryDiagnoser]
public class ParallelForJobBenchmarks
{
    /// <summary>
    /// The number of iterations to perform.
    /// </summary>
    [Params(1000, 100000)]
    public int Iterations { get; set; }

    private int[] _data = null!;
    private Computer _computer = null!;

    /// <summary>
    /// Setup the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _data = new int[Iterations];
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
    /// Baseline: single-threaded sequential execution.
    /// Measures the raw cost of the work with zero scheduling overhead.
    /// </summary>
    [Benchmark(Baseline = true)]
    public void SequentialBaseline()
    {
        Job.ParallelFor(Iterations, Compute, new JobOptions { BatchStrategy = BatchStrategy.None }).Wait();
    }

    /// <summary>
    /// Measure <see cref="Job.ParallelFor"/> with automatic batching.
    /// </summary>
    [Benchmark]
    public void BatchParallelFor()
    {
        Job.ParallelFor(Iterations, Compute).Wait();
    }

    private void Compute(int i)
    {
        var x = (float) i;

        for (var j = 0; j < 100; j++) 
        {
            x = MathF.Sqrt(x + j);
        }

        _data[i] = (int) x;
    }
}