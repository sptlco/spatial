// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// Tests for <see cref="Time"/>.
/// </summary>
public class TimeTests
{
    /// <summary>
    /// Construct a <see cref="Time"/> from a number of milliseconds.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFromMilliseconds()
    {
        var time = Time.FromMilliseconds(5000);

        Assert.Equal(5000, time.Milliseconds);
        Assert.Equal(5, time.Seconds);
    }

    /// <summary>
    /// Construct a <see cref="Time"/> from a number of seconds.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFromSeconds()
    {
        var time = Time.FromSeconds(5);

        Assert.Equal(5000, time.Milliseconds);
        Assert.Equal(5, time.Seconds);
    }

    /// <summary>
    /// Construct a <see cref="Time"/> from a number of minutes.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFromMinutes()
    {
        var time = Time.FromMinutes(1);

        Assert.Equal(60000, time.Milliseconds);
        Assert.Equal(60, time.Seconds);
    }

    /// <summary>
    /// Construct a <see cref="Time"/> from a number of hours.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFromHours()
    {
        var time = Time.FromHours(1);

        Assert.Equal(1 * 60 * 60 * 1000, time.Milliseconds);
        Assert.Equal(1 * 60 * 60, time.Seconds);
    }

    /// <summary>
    /// Construct a <see cref="Time"/> from a number of days.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFromDays()
    {
        var time = Time.FromDays(1);

        Assert.Equal(24 * 60 * 60 * 1000, time.Milliseconds);
        Assert.Equal(24 * 60 * 60, time.Seconds);
    }

    /// <summary>
    /// Construct a <see cref="Time"/> from a <see cref="DateTime"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFromDateTime()
    {
        var time = Time.FromDateTime(new DateTime(2026, 6, 29, 18, 30, 16, DateTimeKind.Utc));

        Assert.Equal(1782757816, time.Seconds);
        Assert.Equal(1782757816000, time.Milliseconds);
    }

    /// <summary>
    /// Construct a <see cref="DateTime"/> from a <see cref="Time"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestToDateTime()
    {
        var time = Time.FromDateTime(new DateTime(2026, 6, 29, 18, 30, 16, DateTimeKind.Utc));
        var date = time.ToDateTime();

        Assert.Equal(2026, date.Year);
        Assert.Equal(6, date.Month);
        Assert.Equal(29, date.Day);
        Assert.Equal(18, date.Hour);
        Assert.Equal(30, date.Minute);
        Assert.Equal(16, date.Second);
    }

    /// <summary>
    /// Get a <see cref="TimeSpan"/> representation of a <see cref="Time"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAsTimeSpan()
    {
        var time = Time.FromMilliseconds(50);
        var span = time.AsTimeSpan();

        Assert.Equal(50, span.TotalMilliseconds);
    }
}
