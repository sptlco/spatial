// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing shortcut data.
/// </summary>
public class SHORT_CUT_DATA : ProtocolBuffer
{
    /// <summary>
    /// The shortcut's slot number.
    /// </summary>
    public byte nSlotNo;

    /// <summary>
    /// The shortcut's code identification number.
    /// </summary>
    public ushort nCodeNo;

    /// <summary>
    /// The shortcut's value.
    /// </summary>
    public int nValue;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nSlotNo = ReadByte();
        nCodeNo = ReadUInt16();
        nValue = ReadInt32();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nSlotNo);
        Write(nCodeNo);
        Write(nValue);
    }
}
