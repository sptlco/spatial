// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute.Commands;

namespace Spatial.Compute;

/// <summary>
/// Tests for a <see cref="Handle"/>.
/// </summary>
public class HandleTests
{
    /// <summary>
    /// Wait for <see cref="Job"/> execution.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestWait()
    {
        using var computer = new Computer();
        using var graph = new Graph();

        graph.Add(new CommandJob(Console.WriteLine));

        using var handle = computer.Dispatch(graph);

        handle.Wait();
    }
}
