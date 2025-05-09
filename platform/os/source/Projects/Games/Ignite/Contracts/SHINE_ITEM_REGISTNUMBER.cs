// Copyright © Spatial. All rights reserved.

using Spatial.Networking;
using System;
using System.Threading;

namespace Ignite.Contracts;

/// <summary>
/// A 64-bit item serial number encoded with temporal and spatial metadata.
/// </summary>
public class SHINE_ITEM_REGISTNUMBER : ProtocolBuffer
{
    /// <summary>
    /// A null <see cref="SHINE_ITEM_REGISTNUMBER"/>.
    /// </summary>
    public static SHINE_ITEM_REGISTNUMBER Null = new(0);

    private static int _counter;

    private ulong _value;

    /// <summary>
    /// Create a new <see cref="SHINE_ITEM_REGISTNUMBER"/>.
    /// </summary>
    public SHINE_ITEM_REGISTNUMBER()
    {
        _value = Generate();
    }

    /// <summary>
    /// Create a new <see cref="SHINE_ITEM_REGISTNUMBER"/>.
    /// </summary>
    /// <param name="value">A 64-bit serial number.</param>
    public SHINE_ITEM_REGISTNUMBER(ulong value)
    {
        _value = value;
    }

    /// <summary>
    /// 4-bit method identifier used during creation
    /// </summary>
    public uint Method => (uint) (_value >> 9 & 0xFu);

    /// <summary>
    /// 8-bit sequence number (cyclical counter)
    /// </summary>
    public uint Sequence => (uint) (_value >> 18 & 0xFFu);

    /// <summary>
    /// Timestamp of generation in UTC
    /// </summary>
    public DateTime Time => new(
        year: 1900 + (int) (_value >> 52 & 0x1Fu),
        month: (int) (_value >> 48 & 0xFu),
        day: (int) (_value >> 43 & 0x1Fu),
        hour: (int) (_value >> 38 & 0x1Fu),
        minute: (int) (_value >> 32 & 0x3Fu),
        second: (int) (_value >> 26 & 0x3Fu)
    );

    /// <summary>
    /// 6-bit world/region identifier
    /// </summary>
    public uint World => (uint) (_value >> 57 & 0x3Fu);

    /// <summary>
    /// 5-bit geographic zone identifier
    /// </summary>
    public uint Zone => (uint) (_value >> 13 & 0x1Fu);

    /// <summary>
    /// Raw 64-bit value representation
    /// </summary>
    public ulong Value => _value;

    /// <summary>
    /// Implicit conversion to raw 64-bit value
    /// </summary>
    public static implicit operator ulong(SHINE_ITEM_REGISTNUMBER serial) => serial._value;

    /// <summary>
    /// Implicit conversion from raw 64-bit value
    /// </summary>
    public static implicit operator SHINE_ITEM_REGISTNUMBER(ulong value) => new(value);

    /// <summary>
    /// Generate a new <see cref="SHINE_ITEM_REGISTNUMBER"/>.
    /// </summary>
    /// <returns>A <see cref="SHINE_ITEM_REGISTNUMBER"/>.</returns>
    public static ulong Generate()
    {
        var now = DateTime.UtcNow;
        var serial = (ulong) 0;

        serial |= 0u & 0x1FFu;
        serial |= (0 & 0xFu) << 9;
        serial |= (0 & 0x1Fu) << 13;
        serial |= (ulong) ((Interlocked.Increment(ref _counter) % byte.MaxValue) & 0xFFu) << 18;
        serial |= (ulong) (now.Second & 0x3Fu) << 26;
        serial |= (ulong) (now.Minute & 0x3Fu) << 32;
        serial |= (ulong) (now.Hour & 0x1Fu) << 38;
        serial |= (ulong) (now.Day & 0x1Fu) << 43;
        serial |= (ulong) (now.Month & 0xFu) << 48;
        serial |= (ulong) ((now.Year - 1900) & 0x1Fu) << 52;
        serial |= (ulong) (0 & 0x3Fu) << 57;
        serial |= (ulong) 0 << 63;

        return serial;
    }

    /// <summary>
    /// Returns hexadecimal string representation
    /// </summary>
    public override string ToString() => $"0x{_value:X16}";

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(_value);
    }

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        _value = ReadUInt64();
    }
}