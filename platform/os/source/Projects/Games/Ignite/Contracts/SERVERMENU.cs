// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing menu item data.
/// </summary>
public class SERVERMENU : ProtocolBuffer
{
    /// <summary>
    /// The item's reply number.
    /// </summary>
    public byte reply;

    /// <summary>
    /// The item's string.
    /// </summary>
    public string str;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        reply = ReadByte();
        str = ReadString(32);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(reply);
        Write(str, 32);
    }
}