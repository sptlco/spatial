// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> that marks partial data.
/// </summary>
public class PARTMARK : ProtocolBuffer
{
    /// <summary>
    /// Whether or not the <see cref="ProtocolBuffer"/> is the first of its group.
    /// </summary>
    public bool bIsStart;

    /// <summary>
    /// Whether or not the <see cref="ProtocolBuffer"/> is the last of its group.
    /// </summary>
    public bool bIsEnd;

    /// <summary>
    /// The order of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public byte nOrder;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        var mark = ReadByte();

        // ...
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        byte mark = 0;

        // ...

        Write(mark);
    }
}
