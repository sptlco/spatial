// Copyright © Spatial. All rights reserved.

using Ignite.Assets.Types;
using Ignite.Models;
using Spatial.Networking;
using Spatial.Persistence;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing avatar information.
/// </summary>
public sealed class PROTO_AVATARINFORMATION : ProtocolBuffer
{
    /// <summary>
    /// The avatar's identification number.
    /// </summary>
    public uint chrregnum;

    /// <summary>
    /// The avatar's name.
    /// </summary>
    public string name;

    /// <summary>
    /// The avatar's level.
    /// </summary>
    public ushort level;

    /// <summary>
    /// The avatar's slot.
    /// </summary>
    public byte slot;

    /// <summary>
    /// The name of the map the avatar is in.
    /// </summary>
    public string loginmap;

    /// <summary>
    /// The deleted avatar's information.
    /// </summary>
    public PROTO_AVATAR_DELETE_INFO delinfo;

    /// <summary>
    /// The avatar's shape.
    /// </summary>
    public PROTO_AVATAR_SHAPE_INFO shape;

    /// <summary>
    /// The avatar's equipment.
    /// </summary>
    public PROTO_EQUIPMENT equip;

    /// <summary>
    /// The avatar's kingdom quest handle.
    /// </summary>
    public uint nKQHandle;

    /// <summary>
    /// The name of the avatar's kingdom quest map.
    /// </summary>
    public string sKQMapName;

    /// <summary>
    /// The avatar's kingdom quest coordinates.
    /// </summary>
    public SHINE_XY_TYPE nKQCoord;

    /// <summary>
    /// The time the avatar's kingdom quest began.
    /// </summary>
    public SHINE_DATETIME dKQDate;

    /// <summary>
    /// The avatar's name change data.
    /// </summary>
    public CHAR_ID_CHANGE_DATA CharIDChangeData;

    /// <summary>
    /// The avatar's tutorial information.
    /// </summary>
    public PROTO_TUTORIAL_INFO TutorialInfo;

    /// <summary>
    /// Create a new <see cref="PROTO_AVATARINFORMATION"/>.
    /// </summary>
    public PROTO_AVATARINFORMATION() { }

    /// <summary>
    /// Create a new <see cref="PROTO_AVATARINFORMATION"/>.
    /// </summary>
    /// <param name="character">A <see cref="Character"/>.</param>
    public PROTO_AVATARINFORMATION(Character character)
    {
        chrregnum = character.Id;
        name = character.Name;
        level = character.Level;
        slot = character.Slot;
        loginmap = Field.Find(character.Map).MapIDClient;
        delinfo = new PROTO_AVATAR_DELETE_INFO();
        shape = new PROTO_AVATAR_SHAPE_INFO {
            race = character.Race,
            chrclass = character.Class,
            gender = character.Gender,
            hairtype = character.Appearance.Hair.Style,
            haircolor = character.Appearance.Hair.Color,
            faceshape = character.Appearance.Face
        };
        equip = new PROTO_EQUIPMENT(character);
        nKQHandle = character.KQ?.Handle ?? 0;
        sKQMapName = character.KQ?.Map ?? string.Empty;
        nKQCoord = new SHINE_XY_TYPE {
            x = (uint) (character.KQ?.Position.X ?? 0),
            y = (uint) (character.KQ?.Position.Y ?? 0)
        };
        dKQDate = new SHINE_DATETIME();
        CharIDChangeData = new CHAR_ID_CHANGE_DATA {
            bNeedChangeID = character.Requirements.Contains(Requirements.Name)
        };
        TutorialInfo = new PROTO_TUTORIAL_INFO {
            nTutorialState = TUTORIAL_STATE.TS_PROGRESS,
            nTutorialStep = 0
        };
    }

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        chrregnum = ReadUInt32();
        name = ReadString(20);
        level = ReadUInt16();
        slot = ReadByte();
        loginmap = ReadString(12);
        delinfo = Read<PROTO_AVATAR_DELETE_INFO>();
        shape = Read<PROTO_AVATAR_SHAPE_INFO>();
        equip = Read<PROTO_EQUIPMENT>();
        nKQHandle = ReadUInt32();
        sKQMapName = ReadString(12);
        nKQCoord = Read<SHINE_XY_TYPE>();
        dKQDate = Read<SHINE_DATETIME>();
        CharIDChangeData = Read<CHAR_ID_CHANGE_DATA>();
        TutorialInfo = Read<PROTO_TUTORIAL_INFO>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(chrregnum);
        Write(name, 20);
        Write(level);
        Write(slot);
        Write(loginmap, 12);
        Write(delinfo);
        Write(shape);
        Write(equip);
        Write(nKQHandle);
        Write(sKQMapName, 12);
        Write(nKQCoord);
        Write(dKQDate);
        Write(CharIDChangeData);
        Write(TutorialInfo);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        delinfo.Dispose();
        shape.Dispose();
        equip.Dispose();
        nKQCoord.Dispose();
        dKQDate.Dispose();
        CharIDChangeData.Dispose();
        TutorialInfo.Dispose();

        base.Dispose();
    }
}
