// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing game option data.
/// </summary>
public class GAME_OPTION_DATA : ProtocolBuffer
{
    /// <summary>
    /// A game option number.
    /// </summary>
    public ushort nOptionNo;

    /// <summary>
    /// The option's value.
    /// </summary>
    public byte nValue;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nOptionNo = ReadUInt16();
        nValue = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>. 
    /// </summary>
    public override void Serialize()
    {
        Write(nOptionNo);
        Write(nValue);
    }
}
