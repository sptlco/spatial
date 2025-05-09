// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Miscellaneous;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_MISC_GAMETIME_ACK"/>.
/// </summary>
public class PROTO_NC_MISC_GAMETIME_ACK : ProtocolBuffer
{
    /// <summary>
    /// The current hour.
    /// </summary>
    public byte hour;

    /// <summary>
    /// The current minute.
    /// </summary>
    public byte minute;

    /// <summary>
    /// The current second.
    /// </summary>
    public byte second;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        hour = ReadByte();
        minute = ReadByte();
        second = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(hour);
        Write(minute);
        Write(second);
    }
}
