// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute.Commands;
using System.Collections.Concurrent;

namespace Spatial.Compute;

/// <summary>
/// Tests for a <see cref="Driver"/>.
/// </summary>
public class DriverTests
{
    /// <summary>
    /// Run the <see cref="Driver"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestRun()
    {
        var output = new ConcurrentDictionary<string, string>();
        var job = new CommandJob(() => output["Status"] = "Success");
        var driver = new Driver(job);

        driver.Run();

        Assert.NotEmpty(output);
        Assert.Single(output);
        Assert.Equal("Success", output["Status"]);
    }
}
