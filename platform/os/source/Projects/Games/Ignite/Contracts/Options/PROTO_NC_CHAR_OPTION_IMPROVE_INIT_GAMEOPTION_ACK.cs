// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_ACK"/>.
/// </summary>
public class PROTO_NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_ACK : ProtocolBuffer
{
    /// <summary>
    /// A classifying error code.
    /// </summary>
    public ushort nError;

    /// <summary>
    /// Game option data.
    /// </summary>
    public PROTO_NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD DBGameOptionData;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nError = ReadUInt16();
        DBGameOptionData = Read<PROTO_NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nError);
        Write(DBGameOptionData);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        DBGameOptionData.Dispose();

        base.Dispose();
    }
}
