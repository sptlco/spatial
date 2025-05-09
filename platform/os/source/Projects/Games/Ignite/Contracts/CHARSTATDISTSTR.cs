// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing stat distribution data.
/// </summary>
public class CHARSTATDISTSTR : ProtocolBuffer
{
    /// <summary>
    /// The character's strength points.
    /// </summary>
    public byte Strength;

    /// <summary>
    /// The character's constitute points.
    /// </summary>
    public byte Constitute;

    /// <summary>
    /// The character's dexterity points.
    /// </summary>
    public byte Dexterity;

    /// <summary>
    /// The character's intelligence points.
    /// </summary>
    public byte Intelligence;

    /// <summary>
    /// The character's mental power points.
    /// </summary>
    public byte MentalPower;

    /// <summary>
    /// The character's redistributable points.
    /// </summary>
    public byte RedistributePoint;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Strength = ReadByte();
        Constitute = ReadByte();
        Dexterity = ReadByte();
        Intelligence = ReadByte();
        MentalPower = ReadByte();
        RedistributePoint = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Strength);
        Write(Constitute);
        Write(Dexterity);
        Write(Intelligence);
        Write(MentalPower);
        Write(RedistributePoint);
    }
}
