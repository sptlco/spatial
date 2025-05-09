// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing coordinate data.
/// </summary>
public class SHINE_XY_TYPE : ProtocolBuffer
{
    /// <summary>
    /// The X-coordinate.
    /// </summary>
    public uint x;

    /// <summary>
    /// The Y-coordinate.
    /// </summary>
    public uint y;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        x = ReadUInt32();
        y = ReadUInt32();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(x);
        Write(y);
    }
}
