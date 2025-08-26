// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Compute.Jobs;

/// <summary>
/// Unit tests for <see cref="Handle"/>.
/// </summary>
public class JobHandleTests
{
    /// <summary>
    /// Test <see cref="Handle.Wait"/>.
    /// </summary>
    [Fact]
    public void TestWait()
    {
        Handle.Create(0).Wait();
    }

    /// <summary>
    /// Test <see cref="Handle.Dispose"/>.
    /// </summary>
    [Fact]
    public void TestDispose()
    {
        Handle.Create(0).Dispose();
    }
}
