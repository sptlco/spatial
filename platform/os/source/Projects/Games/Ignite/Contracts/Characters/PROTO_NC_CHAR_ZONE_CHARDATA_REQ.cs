// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_ZONE_CHARDATA_REQ"/>.
/// </summary>
public class PROTO_NC_CHAR_ZONE_CHARDATA_REQ : ProtocolBuffer
{
    /// <summary>
    /// The character's world manager handle.
    /// </summary>
    public ushort wldmanhandle;

    /// <summary>
    /// The character's name.
    /// </summary>
    public string charid;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        wldmanhandle = ReadUInt16();
        charid = ReadString(20);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(wldmanhandle);
        Write(charid, 20);
    }
}
