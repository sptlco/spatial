// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Objects;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BRIEFINFO_INFORM_CMD"/>.
/// </summary>
public class PROTO_NC_BRIEFINFO_INFORM_CMD : ProtocolBuffer
{
    /// <summary>
    /// The current player's handle.
    /// </summary>
    public ushort nMyHnd;

    /// <summary>
    /// The received <see cref="NETCOMMAND"/>.
    /// </summary>
    public NETCOMMAND ReceiveNetCommand;

    /// <summary>
    /// The target object's handle.
    /// </summary>
    public ushort hnd;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nMyHnd = ReadUInt16();
        ReceiveNetCommand = (NETCOMMAND) ReadUInt16();
        hnd = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nMyHnd);
        Write((ushort) ReceiveNetCommand);
        Write(hnd);
    }
}
