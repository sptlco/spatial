// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_LOGIN_ACK"/>.
/// </summary>
public class PROTO_NC_CHAR_LOGIN_ACK : ProtocolBuffer
{
    /// <summary>
    /// The IP address of the character's connecting zone.
    /// </summary>
    public string zoneip;

    /// <summary>
    /// The port of the character's connecting zone.
    /// </summary>
    public ushort zoneport;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        zoneip = ReadString(16);
        zoneport = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(zoneip, 16);
        Write(zoneport);
    }
}
