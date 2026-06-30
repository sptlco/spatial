// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// Tests for an <see cref="Error"/>.
/// </summary>
public class ErrorTests
{
    /// <summary>
    /// Convert an <see cref="Error"/> to a <see cref="Fault"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public void TestToFault()
    {
        var error = new Unauthorized();
        var fault = error.ToFault();

        Assert.Equal(error, fault.Error);
    }
}
