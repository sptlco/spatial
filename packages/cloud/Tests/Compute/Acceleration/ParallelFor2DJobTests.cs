// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;

namespace Spatial.Compute.Acceleration;

/// <summary>
/// Tests for a <see cref="ParallelFor2DJob"/>.
/// </summary>
public class ParallelFor2DJobTests
{
    /// <summary>
    /// Execute a <see cref="ParallelFor2DJob"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestExecute()
    {
        var output = new ConcurrentDictionary<(int, int), bool>();
        var job = new ParallelFor2DJob(10, 10, 1, 1, (x, y) => output[(x, y)] = true);

        job.Execute(5);

        Assert.Single(output);
        Assert.True(output[(5, 0)]);
    }
}
