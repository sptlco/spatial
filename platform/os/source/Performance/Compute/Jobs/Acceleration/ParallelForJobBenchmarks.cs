// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;

namespace Spatial.Compute.Jobs.Acceleration;

/// <summary>
/// Benchmarks for <see cref="ParallelForJob"/>.
/// </summary>
[GcForce]
[GcServer]
[MemoryDiagnoser]
public class JobParallelForBenchmarks
{
    /// <summary>
    /// The number of iterations to perform.
    /// </summary>
    [Params(1000, 100000)]
    public int Iterations { get; set; }

    private int[] _data = null!;

    /// <summary>
    /// Setup the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
        _data = new int[Iterations];
    }

    /// <summary>
    /// Measure <see cref="Job.ParallelFor"/>.
    /// </summary>
    [Benchmark]
    public void ParallelFor()
    {
        Job.ParallelFor(Iterations, i => _data[i]++, BatchStrategy.None);
    }

    /// <summary>
    /// Measure <see cref="Job.ParallelFor"/> with automatic batching.
    /// </summary>
    [Benchmark]
    public void BatchParallelFor()
    {
        Job.ParallelFor(Iterations, i => _data[i]++);
    }
}