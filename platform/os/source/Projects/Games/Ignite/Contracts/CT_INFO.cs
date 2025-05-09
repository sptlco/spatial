// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing character title data.
/// </summary>
public class CT_INFO : ProtocolBuffer
{
    /// <summary>
    /// The title's type.
    /// </summary>
    public byte Type;

    /// <summary>
    /// The title's element number.
    /// </summary>
    public byte ElementNo;

    /// <summary>
    /// The title's element value.
    /// </summary>
    public byte ElementValue;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Type = ReadByte();

        var element = ReadByte();

        // ...   
    }

    public override void Serialize()
    {
        Write(Type);

        byte element = 0;

        // ...

        Write(element);
    }
}
