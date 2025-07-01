// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;

namespace Spatial.Compute.Jobs.Acceleration;

/// <summary>
/// Unit tests for <see cref="Batch2DJob"/>.
/// </summary>
public class Batch2DJobTests
{
    /// <summary>
    /// Test the <see cref="Batch2DJob"/> constructor.
    /// </summary>
    [Fact]
    public void TestBatch2DJob()
    {
        using var parent = ParallelFor2DJob.Create(5, 10, 1, 1, (_, _) => {});
        using var job = Batch2DJob.Create(parent, 0, 5, 0, 10);

        Assert.Equal(50, job.Size);
        Assert.Equal(parent, job.Parent);
    }

    /// <summary>
    /// Test <see cref="Batch2DJob.Execute"/>.
    /// </summary>
    [Fact]
    public void TestExecute()
    {
        var numbers = new ConcurrentBag<int>();

        using var parent = ParallelFor2DJob.Create(5, 10, 1, 1, (x, y) => numbers.Add(x * y));
        using var job = Batch2DJob.Create(parent, 0, 5, 0, 10);

        job.Execute();

        Assert.Equal(50, numbers.Count);
    }
}
