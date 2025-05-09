// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing charged buff data.
/// </summary>
public class PROTO_CHARGEDBUFF_INFO : ProtocolBuffer
{
    /// <summary>
    /// The buff's key.
    /// </summary>
    public uint ChargedBuffKey;

    /// <summary>
    /// The buff's identification number.
    /// </summary>
    public ushort ChargedBuffID;

    /// <summary>
    /// The time the buff was used.
    /// </summary>
    public ShineDateTime UseTime;

    /// <summary>
    /// The time the buff expires.
    /// </summary>
    public ShineDateTime EndTime;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        ChargedBuffKey = ReadUInt32();
        ChargedBuffID = ReadUInt16();
        UseTime = Read<ShineDateTime>();
        EndTime = Read<ShineDateTime>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(ChargedBuffKey);
        Write(ChargedBuffID);
        Write(UseTime);
        Write(EndTime);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        UseTime.Dispose();
        EndTime.Dispose();

        base.Dispose();
    }
}
