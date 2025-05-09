// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_CLIENT_CHARTITLE_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_CLIENT_CHARTITLE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's current title.
    /// </summary>
    public byte CurrentTitle;

    /// <summary>
    /// The character's current title element.
    /// </summary>
    public byte CurrentTitleElement;

    /// <summary>
    /// The character's current title mob.
    /// </summary>
    public ushort CurrentTitleMobID;

    /// <summary>
    /// The character's current title count.
    /// </summary>
    public ushort NumOfTitle;

    /// <summary>
    /// The character's titles.
    /// </summary>
    public CT_INFO[] TitleArray;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        CurrentTitle = ReadByte();
        CurrentTitleElement = ReadByte();
        CurrentTitleMobID = ReadUInt16();
        NumOfTitle = ReadUInt16();
        TitleArray = Read<CT_INFO>(NumOfTitle);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(CurrentTitle);
        Write(CurrentTitleElement);
        Write(CurrentTitleMobID);
        Write(NumOfTitle);
        Write(TitleArray);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var title in TitleArray)
        {
            title.Dispose();
        }

        base.Dispose();
    }
}
