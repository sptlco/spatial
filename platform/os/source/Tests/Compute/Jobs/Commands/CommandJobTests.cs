// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;

namespace Spatial.Compute.Jobs.Commands;

/// <summary>
/// Unit tests for <see cref="CommandJob"/>.
/// </summary>
public class CommandJobTests
{
    /// <summary>
    /// Test <see cref="CommandJob.Execute"/>.
    /// </summary>
    [Fact]
    public void TestExecute()
    {
        var numbers = new ConcurrentBag<int>();
        using var job = CommandJob.Create(() => numbers.Add(10));

        job.Execute();

        Assert.Single(numbers);
    }
}
