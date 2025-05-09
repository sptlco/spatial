// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing avatar shape information.
/// </summary>
public class PROTO_AVATAR_SHAPE_INFO : ProtocolBuffer
{
    /// <summary>
    /// The avatar's <see cref="Race"/>.
    /// </summary>
    public Race race;

    /// <summary>
    /// The avatar's <see cref="Class"/>.
    /// </summary>
    public Class chrclass;

    /// <summary>
    /// The avatar's <see cref="Gender"/>.
    /// </summary>
    public Gender gender;

    /// <summary>
    /// The avatar's hair type.
    /// </summary>
    public byte hairtype;

    /// <summary>
    /// The avatar's hair color.
    /// </summary>
    public byte haircolor;

    /// <summary>
    /// The avatar's face shape.
    /// </summary>
    public byte faceshape;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        var packed = ReadByte();

        race = (Race) (byte) (packed & 0x03);
        chrclass = (Class) (byte) ((packed >> 2) & 0x1F);
        gender = (Gender) (byte) ((packed >> 7) & 0x01);

        hairtype = ReadByte();
        haircolor = ReadByte();
        faceshape = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        byte packed = 0;
 
        packed |= (byte) ((byte) race & 0x03);
        packed |= (byte) (((byte) chrclass & 0x1F) << 2);
        packed |= (byte) (((byte) gender & 0x01) << 7);

        Write(packed);
        Write(hairtype);
        Write(haircolor);
        Write(faceshape);
    }
}
