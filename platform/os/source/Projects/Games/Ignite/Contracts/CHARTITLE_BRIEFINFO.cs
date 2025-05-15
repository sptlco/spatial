// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing character title data.
/// </summary>
public class CHARTITLE_BRIEFINFO : ProtocolBuffer
{
    /// <summary>
    /// The title's type.
    /// </summary>
    public byte Type;

    /// <summary>
    /// The title's element number.
    /// </summary>
    public byte ElementNo;

    /// <summary>
    /// The title's mob identification number.
    /// </summary>
    public ushort MobID;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Type = ReadByte();
        ElementNo = ReadByte();
        MobID = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Type);
        Write(ElementNo);
        Write(MobID);
    }
}
