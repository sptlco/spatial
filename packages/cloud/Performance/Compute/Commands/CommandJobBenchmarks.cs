// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;

namespace Spatial.Compute.Commands;

/// <summary>
/// Benchmarks for <see cref="CommandJob"/>.
/// </summary>
[GcForce]
[GcServer]
[MemoryDiagnoser]
public class CommandJobBenchmarks
{
    private const int _iterations = 1000;

    private Computer _computer = null!;

    /// <summary>
    /// Setup the benchmarks.
    /// </summary>
    [GlobalSetup]
    public void Setup()
    {
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
    /// Baseline: direct sequential invocation.
    /// Measures the raw cost of calling the action with zero scheduling overhead.
    /// </summary>
    [Benchmark(Baseline = true, OperationsPerInvoke = _iterations)]
    public void DirectInvocation()
    {
        for (var i = 0; i < _iterations; i++)
        {
            EmptyAction();
        }
    }

    /// <summary>
    /// Measure end-to-end cost of scheduling and completing <see cref="_iterations"/>
    /// <see cref="CommandJob"/>s — dispatch, agent pickup, execution, and finalization.
    /// </summary>
    [Benchmark(OperationsPerInvoke = _iterations)]
    public void JobCommand()
    {
        var handles = new Handle[_iterations];

        for (var i = 0; i < _iterations; i++)
        {
            handles[i] = Job.Schedule(EmptyAction);
        }

        for (var i = 0; i < _iterations; i++)
        {
            handles[i].Wait();
        }
    }

    private static void EmptyAction() { }
}