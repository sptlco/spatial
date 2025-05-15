// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Objects;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BRIEFINFO_CHARACTER_CMD"/>. 
/// </summary>
public class PROTO_NC_BRIEFINFO_CHARACTER_CMD : ProtocolBuffer
{
    /// <summary>
    /// The number of characters.
    /// </summary>
    public byte charnum;

    /// <summary>
    /// An array of characters.
    /// </summary>
    public PROTO_NC_BRIEFINFO_LOGINCHARACTER_CMD[] chars;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        charnum = ReadByte();
        chars = Read<PROTO_NC_BRIEFINFO_LOGINCHARACTER_CMD>(charnum);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(charnum);
        Write(chars);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var buffer in chars)
        {
            buffer.Dispose();
        }

        base.Dispose();
    }
}
