// Copyright © Spatial Corporation. All rights reserved.

using BenchmarkDotNet.Attributes;

namespace Spatial.Compute.Jobs.Commands;

/// <summary>
/// Benchmarks for <see cref="CommandJob"/>.
/// </summary>
[GcForce]
[GcServer]
[MemoryDiagnoser]
public class CommandJobBenchmarks
{
    private const int _iterations = 1000;
    
    /// <summary>
    /// Measure a direct invocation of an empty action.
    /// </summary>
    [Benchmark(Baseline = true, OperationsPerInvoke = _iterations)]
    public void DirectInvocation()
    {
        for (int i = 0; i < _iterations; i++)
        {
            EmptyAction();
        }
    }

    /// <summary>
    /// Measure <see cref="Job.Invoke"/>.
    /// </summary>
    [Benchmark(OperationsPerInvoke = _iterations)]
    public void JobCommand()
    {
        for (int i = 0; i < _iterations; i++)
        {
            Job.Invoke(EmptyAction);
        }
    }

    private static void EmptyAction() { }
}
