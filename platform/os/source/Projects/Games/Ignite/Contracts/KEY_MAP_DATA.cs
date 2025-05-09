// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing keymap data.
/// </summary>
public class KEY_MAP_DATA : ProtocolBuffer
{
    /// <summary>
    /// The function identification number.
    /// </summary>
    public ushort nFunctionNo;

    /// <summary>
    /// The key modifier.
    /// </summary>
    public byte nExtendKey;

    /// <summary>
    /// The ASCII code.
    /// </summary>
    public byte nASCIICode;
    
    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nFunctionNo = ReadUInt16();
        nExtendKey = ReadByte();
        nASCIICode = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(nFunctionNo);
        Write(nExtendKey);
        Write(nASCIICode);
    }
}
