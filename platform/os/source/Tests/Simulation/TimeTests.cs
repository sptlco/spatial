// Copyright © Spatial. All rights reserved.

namespace Spatial.Simulation;

/// <summary>
/// Unit tests for <see cref="Time"/>.
/// </summary>
public class TimeTests
{
    /// <summary>
    /// Test <see cref="Time.Zero"/>.
    /// </summary>
    [Fact]
    public void TestZero()
    {
        Assert.Equal(0, Time.Zero.Seconds);
        Assert.Equal(0, Time.Zero);
    }

    /// <summary>
    /// Test <see cref="Time.Now"/>.
    /// </summary>
    [Fact]
    public void TestNow()
    {
        var now = Time.Now;

        Thread.Sleep(50);

        Assert.True(Time.Now > now);
    }

    /// <summary>
    /// Test the <see cref="Time"/> constructor.
    /// </summary>
    [Fact]
    public void TestConstructor()
    {
        var time = new Time(60000);

        Assert.Equal(60, time.Seconds);
        Assert.Equal(60000, time.Milliseconds);
    }
}