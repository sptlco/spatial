// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing coordinate data.
/// </summary>
public class SHINE_COORD_TYPE : ProtocolBuffer
{
    /// <summary>
    /// A positional vector.
    /// </summary>
    public SHINE_XY_TYPE xy;

    /// <summary>
    /// A direction.
    /// </summary>
    public byte dir;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        xy = Read<SHINE_XY_TYPE>();
        dir = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(xy);
        Write(dir);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        xy.Dispose();

        base.Dispose();
    }
}
