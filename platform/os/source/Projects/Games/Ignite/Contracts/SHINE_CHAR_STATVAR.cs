// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing stat variation data.
/// </summary>
public class SHINE_CHAR_STATVAR : ProtocolBuffer
{
    /// <summary>
    /// The stat's base value.
    /// </summary>
    public uint Base;

    /// <summary>
    /// The stat's change in value.
    /// </summary>
    public uint Change;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Base = ReadUInt32();
        Change = ReadUInt32();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Base);
        Write(Change);
    }
}
