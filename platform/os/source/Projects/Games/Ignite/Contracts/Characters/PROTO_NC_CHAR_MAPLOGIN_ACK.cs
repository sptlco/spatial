// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing character login data.
/// </summary>
public class PROTO_NC_CHAR_MAPLOGIN_ACK : ProtocolBuffer
{
    /// <summary>
    /// The character's handle.
    /// </summary>
    public ushort charhandle;

    /// <summary>
    /// The character's parameters.
    /// </summary>
    public CHAR_PARAMETER_DATA param;

    /// <summary>
    /// The character's login coordinates.
    /// </summary>
    public SHINE_XY_TYPE logincoord;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        charhandle = ReadUInt16();
        param = Read<CHAR_PARAMETER_DATA>();
        logincoord = Read<SHINE_XY_TYPE>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(charhandle);
        Write(param);
        Write(logincoord);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        param.Dispose();
        logincoord.Dispose();

        base.Dispose();
    }
}
