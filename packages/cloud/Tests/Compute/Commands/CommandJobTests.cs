// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;

namespace Spatial.Compute.Commands;

/// <summary>
/// Tests for a <see cref="CommandJob"/>.
/// </summary>
public class CommandJobTests
{
    /// <summary>
    /// Execute a <see cref="CommandJob"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestExecute()
    {
        var output = new ConcurrentDictionary<string, string>();
        var job = new CommandJob(() => output["Hello"] = "World");

        job.Execute();

        Assert.Single(output);
        Assert.Equal("World", output["Hello"]);
    }
}
