// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute.Jobs.Acceleration;
using Spatial.Compute.Jobs.Commands;

namespace Spatial.Compute.Jobs;

/// <summary>
/// Unit tests for <see cref="Graph"/>.
/// </summary>
public class GraphTests
{
    /// <summary>
    /// Test the <see cref="Graph"/> constructor.
    /// </summary>
    [Fact]
    public void TestGraph()
    {
        var graph = Graph.Create();

        Assert.Null(graph.Handle);
        Assert.Empty(graph.Jobs);
        Assert.Empty(graph.Dependencies);
        Assert.Empty(graph.Dependants);
    }

    /// <summary>
    /// Test <see cref="Graph.Add"/>.
    /// </summary>
    [Fact]
    public void TestAdd()
    {
        Assert.Single(Graph.Create().Add(CommandJob.Create(() => {})).Jobs);
    }

    /// <summary>
    /// Test <see cref="Graph.Sort"/>.
    /// </summary>
    [Fact]
    public void TestSort()
    {
        var graph = Graph.Create();

        var job1 = CommandJob.Create(() => {});
        var job2 = ParallelForJob.Create(50, 1, Console.WriteLine);
        
        graph.Add(job2);
        graph.Add(job1, job2);

        graph.Sort();

        Assert.Equal(job2, graph.Jobs[0]);
    }
}
