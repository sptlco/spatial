// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// Tests for a <see cref="Profiler"/>.
/// </summary>
public class ProfilerTests
{
    /// <summary>
    /// Measure method execution.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestMeasure()
    {
        var write = Profiler.Measure("Write", Console.WriteLine);

        Assert.Equal("Write", write.Name);
        Assert.True(write.Elapsed > 0);
    }
}
