// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_ABSTATE_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_ABSTATE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's identification number.
    /// </summary>
    public uint chrregnum;

    /// <summary>
    /// The number of abnormal states.
    /// </summary>
    public ushort number;

    /// <summary>
    /// Abnormal states.
    /// </summary>
    public ABSTATEREADBLOCK[] abstate;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        chrregnum = ReadUInt32();
        number = ReadUInt16();
        abstate = Read<ABSTATEREADBLOCK>(number);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(chrregnum);
        Write(number);
        Write(abstate);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var abs in abstate)
        {
            abs.Dispose();
        }

        base.Dispose();
    }
}
