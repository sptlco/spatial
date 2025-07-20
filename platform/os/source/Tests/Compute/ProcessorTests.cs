// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute.Jobs;
using Spatial.Compute.Jobs.Commands;

namespace Spatial.Compute;

/// <summary>
/// Unit tests for <see cref="Processor"/>.
/// </summary>
public class ProcessorTests
{
    /// <summary>
    /// Test <see cref="Processor.Run"/>.
    /// </summary>
    [Fact]
    public void TestRun()
    {
        Processor.Run();

        Assert.NotEmpty(Processor.Agents);
    }

    /// <summary>
    /// Test <see cref="Processor.Dispatch"/>.
    /// </summary>
    [Fact]
    public void TestDispatch()
    {
        using var graph = Graph.Create().Add(CommandJob.Create(() => {}));
        using var handle = Processor.Dispatch(graph);

        Assert.NotNull(handle);
    }

    /// <summary>
    /// Test <see cref="Processor.Finalize"/>.
    /// </summary>
    [Fact]
    public void TestFinalize()
    {
        var graph = Graph.Create();
        var job = CommandJob.Create(() => { });

        graph.Add(job);
        graph.Handle = Handle.Create(1);

        Processor.Finalize(job);
    }
}
