// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Sessions;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_CONNECTION_CHANGE_CHAR_ID_ACK"/>.
/// </summary>
public class PROTO_NC_USER_CONNECTION_CHANGE_CHAR_ID_ACK : ProtocolBuffer
{
    /// <summary>
    /// A classifying error code.
    /// </summary>
    public ushort nError;

    /// <summary>
    /// The avatar's slot number.
    /// </summary>
    public byte nSlotNo;

    /// <summary>
    /// The avatar's new name.
    /// </summary>
    public string sNewID;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nError = ReadUInt16();
        nSlotNo = ReadByte();
        sNewID = ReadString(20);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nError);
        Write(nSlotNo);
        Write(sNewID, 20);
    }
}
