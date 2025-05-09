// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> that contains an item's option count.
/// </summary>
public class ItemOptionNumber : ProtocolBuffer
{
    /// <summary>
    /// Undocumented.
    /// </summary>
    public bool identify;

    /// <summary>
    /// The number of options the item has.
    /// </summary>
    public byte optionnum;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        var bin = ReadByte();

        identify = (bin & 0x1) != 0;
        optionnum = (byte) ((bin >> 1) & 0x7F);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write((byte) (((optionnum & 0x7F) << 1) | ((identify ? 1 : 0) & 0x1)));
    }
}