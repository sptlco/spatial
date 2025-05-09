// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Miscellaneous;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_MISC_RESTMINUTE_CMD"/>.
/// </summary>
public class PROTO_NC_MISC_RESTMINUTE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The timer's flags.
    /// </summary>
    public byte flag;

    /// <summary>
    /// The remaining seconds.
    /// </summary>
    public ushort second;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        flag = ReadByte();
        second = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(flag);
        Write(second);
    }
}
