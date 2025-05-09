// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Menu;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_MENU_SERVERMENU_ACK"/>.
/// </summary>
public class PROTO_NC_MENU_SERVERMENU_ACK : ProtocolBuffer
{
    /// <summary>
    /// The player's reply.
    /// </summary>
    public byte reply;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        reply = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(reply);
    }
}
