// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Compute;

/// <summary>
/// Tests for an <see cref="Agent"/>.
/// </summary>
public class AgentTests
{
    /// <summary>
    /// Run the <see cref="Agent"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestRun()
    {
        using var computer = new Computer();
        using var agent = new Agent(computer, 999);

        agent.Run();
    }

    /// <summary>
    /// Wake the <see cref="Agent"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestWake()
    {
        using var computer = new Computer();
        using var agent = new Agent(computer, 999);

        agent.Wake();
    }

    /// <summary>
    /// Dispose of an <see cref="Agent"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestDispose()
    {
        using var computer = new Computer();
        var agent = new Agent(computer, 999);

        agent.Dispose();
    }
}