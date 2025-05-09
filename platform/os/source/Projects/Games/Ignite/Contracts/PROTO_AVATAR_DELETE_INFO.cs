// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing deleted avatar information.
/// </summary>
public class PROTO_AVATAR_DELETE_INFO : ProtocolBuffer
{
    /// <summary>
    /// The year the avatar was deleted.
    /// </summary>
    public byte year;

    /// <summary>
    /// The month the avatar was deleted.
    /// </summary>
    public byte month;

    /// <summary>
    /// The day the avatar was deleted.
    /// </summary>
    public byte day;

    /// <summary>
    /// The hour the avatar was deleted.
    /// </summary>
    public byte hour;

    /// <summary>
    /// The minute the avatar was deleted.
    /// </summary>
    public byte min;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        year = ReadByte();
        month = ReadByte();
        day = ReadByte();
        hour = ReadByte();
        min = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(year);
        Write(month);
        Write(day);
        Write(hour);
        Write(min);
    }
}
