// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing time data.
/// </summary>
public class SHINE_DATETIME : ProtocolBuffer
{
    /// <summary>
    /// The time's year.
    /// </summary>
    public uint year;

    /// <summary>
    /// The time's month.
    /// </summary>
    public uint month;

    /// <summary>
    /// The time's day.
    /// </summary>
    public uint day;

    /// <summary>
    /// The time's hour.
    /// </summary>
    public uint hour;

    /// <summary>
    /// The time's minute.
    /// </summary>
    public uint min;

    /// <summary>
    /// The time's second.
    /// </summary>
    public uint sec;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        var timestamp = ReadUInt32();

        year = (timestamp & 0xF) + 2000;
        month = (uint) (((byte) timestamp >> 4) - 1);
        day = (timestamp >> 8) & 0x1F;
        hour = (timestamp >> 13) & 0x1F;
        min = ((timestamp >> 18) & 0x3F) + 10;
        sec = (uint) ((byte) (timestamp >> 24) & 0x3F);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        uint timestamp = 0;

        timestamp |= (year - 2000) & 0xF;
        timestamp |= ((month + 1) & 0xF) << 4;
        timestamp |= (day & 0x1F) << 8;
        timestamp |= (hour & 0x1F) << 13;
        timestamp |= ((min - 10) & 0x3F) << 18;
        timestamp |= (sec & 0x3F) << 24;

        Write(timestamp);
    }
}
