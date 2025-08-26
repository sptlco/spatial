// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;

namespace Spatial.Compute.Jobs.Acceleration;

/// <summary>
/// Unit tests for <see cref="BatchJob"/>.
/// </summary>
public class BatchJobTests
{
    /// <summary>
    /// Test the <see cref="BatchJob"/> constructor.
    /// </summary>
    [Fact]
    public void TestBatchJob()
    {
        using var parent = ParallelForJob.Create(50, 10, _ => {});
        using var job = BatchJob.Create(parent, 0, 10);

        Assert.Equal(10, job.Size);
        Assert.Equal(parent, job.Parent);
    }

    /// <summary>
    /// Test <see cref="BatchJob.Execute"/>.
    /// </summary>
    [Fact]
    public void TestExecute()
    {
        var numbers = new ConcurrentBag<int>();

        using var parent = ParallelForJob.Create(50, 10, numbers.Add);
        using var job = BatchJob.Create(parent, 0, 10);

        job.Execute();

        Assert.Equal(10, numbers.Count);
    }
}
