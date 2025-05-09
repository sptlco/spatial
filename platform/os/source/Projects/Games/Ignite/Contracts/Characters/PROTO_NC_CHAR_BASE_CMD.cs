// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_BASE_CMD"/>.
/// </summary>
public class PROTO_NC_CHAR_BASE_CMD : ProtocolBuffer
{
    /// <summary>
    /// The character's identification number.
    /// </summary>
    public uint chrregnum;

    /// <summary>
    /// The character's name.
    /// </summary>
    public string charid;

    /// <summary>
    /// The character's slot number.
    /// </summary>
    public byte slotno;

    /// <summary>
    /// The character's level.
    /// </summary>
    public byte Level;

    /// <summary>
    /// The character's current XP.
    /// </summary>
    public ulong Experience;

    /// <summary>
    /// The character's power stone count.
    /// </summary>
    public ushort CurPwrStone;

    /// <summary>
    /// The character's guard stone count.
    /// </summary>
    public ushort CurGrdStone;

    /// <summary>
    /// The character's HP stone count.
    /// </summary>
    public ushort CurHPStone;

    /// <summary>
    /// The character's SP stone count.
    /// </summary>
    public ushort CurSPStone;

    /// <summary>
    /// The character's current health points.
    /// </summary>
    public uint CurHP;

    /// <summary>
    /// The character's current spirit points.
    /// </summary>
    public uint CurSP;

    /// <summary>
    /// The character's current light points.
    /// </summary>
    public uint CurLP;

    /// <summary>
    /// The character's current fame.
    /// </summary>
    public uint fame;

    /// <summary>
    /// The character's current copper balance.
    /// </summary>
    public ulong Cen;

    /// <summary>
    /// The character's location.
    /// </summary>
    public LoginLocation logininfo;

    /// <summary>
    /// The character's stat distribution.
    /// </summary>
    public CHARSTATDISTSTR statdistribute;

    /// <summary>
    /// The character's PK cooldown.
    /// </summary>
    public byte pkyellowtime;

    /// <summary>
    /// The character's player kill count.
    /// </summary>
    public uint pkcount;

    /// <summary>
    /// The number of minutes the character is imprisoned for.
    /// </summary>
    public ushort prisonmin;

    /// <summary>
    /// The character's admin level.
    /// </summary>
    public byte adminlevel;

    /// <summary>
    /// The character's flags.
    /// </summary>
    public Flags flags;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        chrregnum = ReadUInt32();
        charid = ReadString(20);
        slotno = ReadByte();
        Level = ReadByte();
        Experience = ReadUInt64();
        CurPwrStone = ReadUInt16();
        CurGrdStone = ReadUInt16();
        CurHPStone = ReadUInt16();
        CurSPStone = ReadUInt16();
        CurHP = ReadUInt32();
        CurSP = ReadUInt32();
        CurLP = ReadUInt32();
        fame = ReadUInt16();
        Cen = ReadUInt64();
        logininfo = Read<LoginLocation>();
        statdistribute = Read<CHARSTATDISTSTR>();
        pkyellowtime = ReadByte();
        pkcount = ReadUInt32();
        prisonmin = ReadUInt16();
        adminlevel = ReadByte();
        flags = Read<Flags>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(chrregnum);
        Write(charid, 20);
        Write(slotno);
        Write(Level);
        Write(Experience);
        Write(CurPwrStone);
        Write(CurGrdStone);
        Write(CurHPStone);
        Write(CurSPStone);
        Write(CurHP);
        Write(CurSP);
        Write(CurLP);
        Write(fame);
        Write(Cen);
        Write(logininfo);
        Write(statdistribute);
        Write(pkyellowtime);
        Write(pkcount);
        Write(prisonmin);
        Write(adminlevel);
        Write(flags);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        logininfo.Dispose();
        statdistribute.Dispose();
        flags.Dispose();

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing location data for a character.
    /// </summary>
    public class LoginLocation : ProtocolBuffer
    {
        /// <summary>
        /// The character's current map.
        /// </summary>
        public string currentmap;

        /// <summary>
        /// The character's coordinates.
        /// </summary>
        public SHINE_COORD_TYPE currentcoord;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            currentmap = ReadString(12);
            currentcoord = Read<SHINE_COORD_TYPE>();
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(currentmap, 12);
            Write(currentcoord);
        }

        /// <summary>
        /// Dispose of the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Dispose()
        {
            currentcoord.Dispose();
        }
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing flags for a character.
    /// </summary>
    public class Flags : ProtocolBuffer
    {
        /// <summary>
        /// Binary data.
        /// </summary>
        public uint bin;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            bin = ReadUInt32();
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(bin);
        }
    }
}
