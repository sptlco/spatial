// Copyright © Spatial. All rights reserved.

using Spatial.Networking.Contracts.Miscellaneous;

namespace Spatial.Networking.Contracts;

/// <summary>
/// Unit tests for miscellaneous contracts.
/// </summary>
public class MiscellaneousContractTests
{
    /// <summary>
    /// Test <see cref="PROTO_NC_MISC_SEED_CMD"/>.
    /// </summary>
    [Fact]
    public void PROTO_NC_MISC_SEED_CMD()
    {
        var buffer = new PROTO_NC_MISC_SEED_CMD(5);

        Assert.Equal(5, buffer.Seed);

        buffer.Serialize();

        var array = buffer.ToArray();

        Assert.Equal(2, array.Length);

        buffer = ProtocolBuffer.FromSpan<PROTO_NC_MISC_SEED_CMD>([4, 0]);

        Assert.Equal(4, buffer.Seed);
    }
}