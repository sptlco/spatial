// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute.Acceleration;
using Spatial.Compute.Commands;

namespace Spatial.Compute;

/// <summary>
/// Tests for a <see cref="Graph"/>.
/// </summary>
public class GraphTests
{
    /// <summary>
    /// Add a <see cref="Job"/> to the <see cref="Graph"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAdd()
    {
        var job = new CommandJob(Console.WriteLine);
        using var graph = new Graph();

        graph.Add(job);

        Assert.Equal(graph.Jobs[0], job);
        Assert.Single(graph.Jobs);
        Assert.NotEmpty(graph.Jobs);
    }

    /// <summary>
    /// Sort a <see cref="Graph"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestSort()
    {
        using var graph = new Graph();

        var job1 = new CommandJob(Console.WriteLine);
        var job2 = new ParallelForJob(50, 1, Console.WriteLine);

        graph.Add(job2);
        graph.Add(job1, job2);

        graph.Sort();

        Assert.Equal(job2, graph.Jobs[0]);
    }
}
