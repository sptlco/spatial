// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute.Commands;

namespace Spatial.Compute;

/// <summary>
/// Tests for a <see cref="Computer"/>.
/// </summary>
public class ComputerTests
{
    /// <summary>
    /// Dispatch a <see cref="Graph"/> to the <see cref="Computer"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestDispatch()
    {
        using var computer = new Computer();
        using var graph = new Graph();

        graph.Add(new CommandJob(() => {
            File.WriteAllText("TestDispatch.txt", "Success");
        }));

        computer.Dispatch(graph).Wait();

        Assert.True(File.Exists("TestDispatch.txt"));
        Assert.Equal("Success", File.ReadAllText("TestDispatch.txt"));

        File.Delete("TestDispatch.txt");
    }

    /// <summary>
    /// Dispose of a <see cref="Computer"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestDispose()
    {
        using var computer = new Computer();
    }
}