// Copyright © Spatial. All rights reserved.

namespace Spatial.Compute.Jobs.Acceleration;

/// <summary>
/// Unit tests for <see cref="ParallelForJob"/>.
/// </summary>
public class ParallelForJobTests
{
    /// <summary>
    /// Test the <see cref="ParallelForJob"/> constructor.
    /// </summary>
    [Fact]
    public void TestParallelForJob()
    {
        using var job = ParallelForJob.Create(10, 2, (i) => {});

        Assert.Equal(10, job.Iterations);
        Assert.Equal(1, job.BatchSize);
    }

    /// <summary>
    /// Test <see cref="ParallelForJob.Execute"/>.
    /// </summary>
    [Fact]
    public void TestExecute()
    {
        var numbers = new List<int>();
        using var job = ParallelForJob.Create(10, 2, numbers.Add);

        job.Execute(3);

        Assert.Single(numbers);
        Assert.Equal(3, numbers[0]);
    }

    /// <summary>
    /// Test <see cref="ParallelForJob.CalculateBatchSize"/>.
    /// </summary>
    [Fact]
    public void TestCalculateBatchSize()
    {
        Assert.True(ParallelForJob.CalculateBatchSize(10, 1) >= 1);
    }
}
