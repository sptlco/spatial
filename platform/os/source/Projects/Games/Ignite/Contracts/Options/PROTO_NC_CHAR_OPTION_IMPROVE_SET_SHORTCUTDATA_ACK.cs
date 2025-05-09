// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Options;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_ACK"/>.
/// </summary>
public class PROTO_NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_ACK : ProtocolBuffer
{
    /// <summary>
    /// A classifying error code.
    /// </summary>
    public ushort nError;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nError = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nError);
    }
}
