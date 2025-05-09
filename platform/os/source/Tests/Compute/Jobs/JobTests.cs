// Copyright © Spatial. All rights reserved.

using System.Collections.Concurrent;

namespace Spatial.Compute.Jobs;

/// <summary>
/// Unit tests for <see cref="Job"/>.
/// </summary>
public class JobTests
{
    /// <summary>
    /// Test <see cref="Job.Accelerator"/>.
    /// </summary>
    [Fact]
    public void TestAccelerator()
    {
        Assert.NotNull(Job.Accelerator());
    }

    /// <summary>
    /// Test <see cref="Job.Command"/>.
    /// </summary>
    [Fact]
    public void TestCommand()
    {
        Job.Command(() => Console.WriteLine("Hello, world!"));
    }

    /// <summary>
    /// Test <see cref="Job.ParallelFor"/>.
    /// </summary>
    [Fact]
    public void TestParallelFor()
    {
        var numbers = new ConcurrentBag<int>();

        Job.ParallelFor(500, numbers.Add);

        Assert.Equal(500, numbers.Count);
    }

    /// <summary>
    /// Test <see cref="Job.ParallelFor2D"/>.
    /// </summary>
    [Fact]
    public void TestParallelFor2D()
    {
        var numbers = new ConcurrentBag<int>();

        Job.ParallelFor2D(10, 50, (x, y) => numbers.Add(x * y));

        Assert.Equal(500, numbers.Count);
    }
}