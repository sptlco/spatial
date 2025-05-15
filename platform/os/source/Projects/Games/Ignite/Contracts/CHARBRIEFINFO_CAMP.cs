// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing character data.
/// </summary>
public class CHARBRIEFINFO_CAMP : ProtocolBuffer
{
    /// <summary>
    /// The character's minihouse.
    /// </summary>
    public ushort minihouse;

    /// <summary>
    /// Dummy data.
    /// </summary>
    public byte[] dummy;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        minihouse = ReadUInt16();
        dummy = ReadBytes(10);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(minihouse);
        Write(dummy);
    }
}
