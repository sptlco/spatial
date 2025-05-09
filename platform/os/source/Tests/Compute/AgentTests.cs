// Copyright © Spatial. All rights reserved.


// Copyright © Spatial. All rights reserved.

namespace Spatial.Compute;

/// <summary>
/// Unit tests for an <see cref="Agent"/>.
/// </summary>
public class AgentTests
{
    /// <summary>
    /// Test the <see cref="Agent"/> constructor.
    /// </summary>
    [Fact]
    public void TestAgent()
    {
        var agent = new Agent(4);

        Assert.NotNull(agent.Queue);
    }
}