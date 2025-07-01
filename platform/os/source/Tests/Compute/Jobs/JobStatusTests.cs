// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Compute.Jobs;

/// <summary>
/// Unit tests for <see cref="JobStatus"/>.
/// </summary>
public class JobStatusTests
{
    /// <summary>
    /// Test <see cref="JobStatus"/> enumeration.
    /// </summary>
    [Fact]
    public void TestJobStatus()
    {
        Assert.Equal(0, (int) JobStatus.Submitted);
        Assert.Equal(1, (int) JobStatus.Scheduled);
        Assert.Equal(2, (int) JobStatus.Running);
        Assert.Equal(3, (int) JobStatus.Complete);
        Assert.Equal(4, (int) JobStatus.Failed);
    }
}
