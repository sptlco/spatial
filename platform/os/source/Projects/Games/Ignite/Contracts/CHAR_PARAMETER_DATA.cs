// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing character parameter data.
/// </summary>
public class CHAR_PARAMETER_DATA : ProtocolBuffer
{
    /// <summary>
    /// The character's previous XP.
    /// </summary>
    public ulong PrevExp;

    /// <summary>
    /// The character's next XP.
    /// </summary>
    public ulong NextExp;

    /// <summary>
    /// The character's strength.
    /// </summary>
    public SHINE_CHAR_STATVAR Strength;

    /// <summary>
    /// The character's constitute.
    /// </summary>
    public SHINE_CHAR_STATVAR Constitute;

    /// <summary>
    /// The character's dexterity.
    /// </summary>
    public SHINE_CHAR_STATVAR Dexterity;

    /// <summary>
    /// The character's intelligence.
    /// </summary>
    public SHINE_CHAR_STATVAR Intelligence;

    /// <summary>
    /// The character's wisdom.
    /// </summary>
    public SHINE_CHAR_STATVAR Wizdom;

    /// <summary>
    /// The character's mental power.
    /// </summary>
    public SHINE_CHAR_STATVAR MentalPower;

    /// <summary>
    /// The character's minimum damage.
    /// </summary>
    public SHINE_CHAR_STATVAR WClow;

    /// <summary>
    /// The character's maximum damage.
    /// </summary>
    public SHINE_CHAR_STATVAR WChigh;

    /// <summary>
    /// The character's defense.
    /// </summary>
    public SHINE_CHAR_STATVAR AC;

    /// <summary>
    /// The character's aim.
    /// </summary>
    public SHINE_CHAR_STATVAR TH;

    /// <summary>
    /// The character's evasion.
    /// </summary>
    public SHINE_CHAR_STATVAR TB;

    /// <summary>
    /// The character's minimum magic damage.
    /// </summary>
    public SHINE_CHAR_STATVAR MAlow;

    /// <summary>
    /// The character's maximum magic damage.
    /// </summary>
    public SHINE_CHAR_STATVAR MAhigh;

    /// <summary>
    /// The character's magic defense.
    /// </summary>
    public SHINE_CHAR_STATVAR MR;

    /// <summary>
    /// The character's magic aim.
    /// </summary>
    public SHINE_CHAR_STATVAR MH = new();

    /// <summary>
    /// The character's magic evasion.
    /// </summary>
    public SHINE_CHAR_STATVAR MB = new();

    /// <summary>
    /// The character's maximum health points.
    /// </summary>
    public uint MaxHp;

    /// <summary>
    /// The character's maximum spirit points.
    /// </summary>
    public uint MaxSp;

    /// <summary>
    /// The character's maximum light points.
    /// </summary>
    public uint MaxLp;

    /// <summary>
    /// The character's maximum attack points.
    /// </summary>
    public uint MaxAp;

    /// <summary>
    /// The character's maximum HP stone count.
    /// </summary>
    public uint MaxHPStone;

    /// <summary>
    /// The character's maximum SP stone count.
    /// </summary>
    public uint MaxSPStone;

    /// <summary>
    /// The character's power stones.
    /// </summary>
    public STONE PwrStone = new();

    /// <summary>
    /// The character's guard stones.
    /// </summary>
    public STONE GrdStone = new();

    /// <summary>
    /// The character's pain resistance.
    /// </summary>
    public SHINE_CHAR_STATVAR PainRes;

    /// <summary>
    /// The character's restraint resistance.
    /// </summary>
    public SHINE_CHAR_STATVAR RestraintRes;

    /// <summary>
    /// The character's curse resistance.
    /// </summary>
    public SHINE_CHAR_STATVAR CurseRes;

    /// <summary>
    /// The character's stun resistance.
    /// </summary>
    public SHINE_CHAR_STATVAR ShockRes;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        PrevExp = ReadUInt64();
        NextExp = ReadUInt64();
        Strength = Read<SHINE_CHAR_STATVAR>();
        Constitute = Read<SHINE_CHAR_STATVAR>();
        Dexterity = Read<SHINE_CHAR_STATVAR>();
        Intelligence = Read<SHINE_CHAR_STATVAR>();
        Wizdom = Read<SHINE_CHAR_STATVAR>();
        MentalPower = Read<SHINE_CHAR_STATVAR>();
        WClow = Read<SHINE_CHAR_STATVAR>();
        WChigh = Read<SHINE_CHAR_STATVAR>();
        AC = Read<SHINE_CHAR_STATVAR>();
        TH = Read<SHINE_CHAR_STATVAR>();
        TB = Read<SHINE_CHAR_STATVAR>();
        MAlow = Read<SHINE_CHAR_STATVAR>();
        MAhigh = Read<SHINE_CHAR_STATVAR>();
        MR = Read<SHINE_CHAR_STATVAR>();
        MH = Read<SHINE_CHAR_STATVAR>();
        MB = Read<SHINE_CHAR_STATVAR>();
        MaxHp = ReadUInt32();
        MaxSp = ReadUInt32();
        MaxLp = ReadUInt32();
        MaxAp = ReadUInt32();
        MaxHPStone = ReadUInt32();
        MaxSPStone = ReadUInt32();
        PwrStone = Read<STONE>();
        GrdStone = Read<STONE>();
        PainRes = Read<SHINE_CHAR_STATVAR>();
        RestraintRes = Read<SHINE_CHAR_STATVAR>();
        CurseRes = Read<SHINE_CHAR_STATVAR>();
        ShockRes = Read<SHINE_CHAR_STATVAR>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(PrevExp);
        Write(NextExp);
        Write(Strength);
        Write(Constitute);
        Write(Dexterity);
        Write(Intelligence);
        Write(Wizdom);
        Write(MentalPower);
        Write(WClow);
        Write(WChigh);
        Write(AC);
        Write(TH);
        Write(TB);
        Write(MAlow);
        Write(MAhigh);
        Write(MR);
        Write(MH);
        Write(MB);
        Write(MaxHp);
        Write(MaxSp);
        Write(MaxLp);
        Write(MaxAp);
        Write(MaxHPStone);
        Write(MaxSPStone);
        Write(PwrStone);
        Write(GrdStone);
        Write(PainRes);
        Write(RestraintRes);
        Write(CurseRes);
        Write(ShockRes);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        Strength.Dispose();
        Constitute.Dispose();
        Dexterity.Dispose();
        Intelligence.Dispose();
        Wizdom.Dispose();
        MentalPower.Dispose();
        WClow.Dispose();
        WChigh.Dispose();
        AC.Dispose();
        TH.Dispose();
        TB.Dispose();
        MAlow.Dispose();
        MAhigh.Dispose();
        MR.Dispose();
        MH.Dispose();
        MB.Dispose();
        PwrStone.Dispose();
        GrdStone.Dispose();
        PainRes.Dispose();
        RestraintRes.Dispose();
        CurseRes.Dispose();
        ShockRes.Dispose();

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing stone data.
    /// </summary>
    public class STONE : ProtocolBuffer
    {
        /// <summary>
        /// The stone's flag.
        /// </summary>
        public uint flag;

        /// <summary>
        /// Physical stones.
        /// </summary>
        public uint EPPysic;

        /// <summary>
        /// Magic stones.
        /// </summary>
        public uint EPMagic;

        /// <summary>
        /// The maximum stone count.
        /// </summary>
        public uint MaxStone;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            flag = ReadUInt32();
            EPPysic = ReadUInt32();
            EPMagic = ReadUInt32();
            MaxStone = ReadUInt32();
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(flag);
            Write(EPPysic);
            Write(EPMagic);
            Write(MaxStone);
        }
    }
}
