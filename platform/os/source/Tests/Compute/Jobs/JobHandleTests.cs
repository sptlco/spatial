// Copyright © Spatial. All rights reserved.

namespace Spatial.Compute.Jobs;

/// <summary>
/// Unit tests for <see cref="JobHandle"/>.
/// </summary>
public class JobHandleTests
{
    /// <summary>
    /// Test <see cref="JobHandle.Wait"/>.
    /// </summary>
    [Fact]
    public void TestWait()
    {
        JobHandle.Create(0).Wait();
    }

    /// <summary>
    /// Test <see cref="JobHandle.Dispose"/>.
    /// </summary>
    [Fact]
    public void TestDispose()
    {
        JobHandle.Create(0).Dispose();
    }
}
