// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Compute.Jobs.Acceleration;

/// <summary>
/// Unit tests for <see cref="ParallelFor2DJob"/>.
/// </summary>
public class ParallelFor2DJobTests
{
    /// <summary>
    /// Test the <see cref="ParallelFor2DJob"/> constructor.
    /// </summary>
    [Fact]
    public void TestParallelFor2DJob()
    {
        using var job = ParallelFor2DJob.Create(10, 2, 3, 4, (x, y) => {});

        Assert.Equal(20, job.Iterations);
        Assert.Equal(10, job.Width);
        Assert.Equal(2, job.Height);
        Assert.Equal(1, job.BatchSizeX);
        Assert.Equal(1, job.BatchSizeY);
    }

    /// <summary>
    /// Test <see cref="ParallelFor2DJob.Execute"/>.
    /// </summary>
    [Fact]
    public void TestExecute()
    {
        var numbers = new List<int>();
        using var job = ParallelFor2DJob.Create(2, 2, 0, 0, (x, y) => numbers.Add(x * y));

        job.Execute(3);

        Assert.Single(numbers);
        Assert.Equal(1, numbers[0]);
    }
}
