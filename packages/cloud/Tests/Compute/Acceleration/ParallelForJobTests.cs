// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;

namespace Spatial.Compute.Acceleration;

/// <summary>
/// Tests for a <see cref="ParallelForJob"/>.
/// </summary>
public class ParallelForJobTests
{
    /// <summary>
    /// Execute a <see cref="ParallelForJob"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestExecute()
    {
        var output = new ConcurrentDictionary<int, bool>();
        var job = new ParallelForJob(5, 1, i => output[i] = true);

        job.Execute(3);

        Assert.Single(output);
        Assert.True(output[3]);
    }
}
