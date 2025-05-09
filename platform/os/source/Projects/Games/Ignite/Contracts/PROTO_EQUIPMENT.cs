// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing equipment data.
/// </summary>
public sealed class PROTO_EQUIPMENT : ProtocolBuffer
{
    /// <summary>
    /// The equipped head item.
    /// </summary>
    public ushort Equ_Head;

    /// <summary>
    /// The equipped mouth item.
    /// </summary>
    public ushort Equ_Mouth;

    /// <summary>
    /// The equipped right-hand item.
    /// </summary>
    public ushort Equ_RightHand;

    /// <summary>
    /// The equipped body item.
    /// </summary>
    public ushort Equ_Body;

    /// <summary>
    /// The equipped left-hand item.
    /// </summary>
    public ushort Equ_LeftHand;

    /// <summary>
    /// The equipped pants item.
    /// </summary>
    public ushort Equ_Pant;

    /// <summary>
    /// The equipped boots item.
    /// </summary>
    public ushort Equ_Boot;

    /// <summary>
    /// The equipped boots accessory item.
    /// </summary>
    public ushort Equ_AccBoot;

    /// <summary>
    /// The equipped pants accessory item.
    /// </summary>
    public ushort Equ_AccPant;

    /// <summary>
    /// The equipped body accessory item.
    /// </summary>
    public ushort Equ_AccBody;

    /// <summary>
    /// The equipped head accessory item.
    /// </summary>
    public ushort Equ_AccHeadA;

    /// <summary>
    /// The equipped right-shoulder minipet item.
    /// </summary>
    public ushort Equ_MiniMon_R;

    /// <summary>
    /// The equipped eye item.
    /// </summary>
    public ushort Equ_Eye;

    /// <summary>
    /// The equipped left-hand accessory item.
    /// </summary>
    public ushort Equ_AccLeftHand;

    /// <summary>
    /// The equipped right-hand accessory item.
    /// </summary>
    public ushort Equ_AccRightHand;

    /// <summary>
    /// The equipped back accessory item.
    /// </summary>
    public ushort Equ_AccBack;

    /// <summary>
    /// The equipped FX item.
    /// </summary>
    public ushort Equ_CosEff;

    /// <summary>
    /// The equipped hip accessory item.
    /// </summary>
    public ushort Equ_AccHip;

    /// <summary>
    /// The equipped left-shoulder minipet item.
    /// </summary>
    public ushort Equ_Minimon;

    /// <summary>
    /// The equipped shield accessory item.
    /// </summary>
    public ushort Equ_AccShield;

    /// <summary>
    /// Equipped item upgrades.
    /// </summary>
    public PROTO_UPGRADES upgrade;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Equ_Head = ReadUInt16();
        Equ_Mouth = ReadUInt16();
        Equ_RightHand = ReadUInt16();
        Equ_Body = ReadUInt16();
        Equ_LeftHand = ReadUInt16();
        Equ_Pant = ReadUInt16();
        Equ_Boot = ReadUInt16();
        Equ_AccBoot = ReadUInt16();
        Equ_AccPant = ReadUInt16();
        Equ_AccBody = ReadUInt16();
        Equ_AccHeadA = ReadUInt16();
        Equ_MiniMon_R = ReadUInt16();
        Equ_Eye = ReadUInt16();
        Equ_AccLeftHand = ReadUInt16();
        Equ_AccRightHand = ReadUInt16();
        Equ_AccBack = ReadUInt16();
        Equ_CosEff = ReadUInt16();
        Equ_AccHip = ReadUInt16();
        Equ_Minimon = ReadUInt16();
        Equ_AccShield = ReadUInt16();
        upgrade = Read<PROTO_UPGRADES>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Equ_Head);
        Write(Equ_Mouth);
        Write(Equ_RightHand);
        Write(Equ_Body);
        Write(Equ_LeftHand);
        Write(Equ_Pant);
        Write(Equ_Boot);
        Write(Equ_AccBoot);
        Write(Equ_AccPant);
        Write(Equ_AccBody);
        Write(Equ_AccHeadA);
        Write(Equ_MiniMon_R);
        Write(Equ_Eye);
        Write(Equ_AccLeftHand);
        Write(Equ_AccRightHand);
        Write(Equ_AccBack);
        Write(Equ_CosEff);
        Write(Equ_AccHip);
        Write(Equ_Minimon);
        Write(Equ_AccShield);
        Write(upgrade);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        upgrade.Dispose();

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing equipment upgrades.
    /// </summary>
    public class PROTO_UPGRADES : ProtocolBuffer
    {
        /// <summary>
        /// Left-hand item upgrades.
        /// </summary>
        public byte lefthand;

        /// <summary>
        /// Right-hand item upgrades.
        /// </summary>
        public byte righthand;

        /// <summary>
        /// Body item upgrades.
        /// </summary>
        public byte body;

        /// <summary>
        /// Leg item upgrades.
        /// </summary>
        public byte leg;

        /// <summary>
        /// Shoe upgrades.
        /// </summary>
        public byte shoes;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            var packed1 = ReadByte();
            var packed2 = ReadByte();
            var packed3 = ReadByte();

            lefthand = (byte) (packed1 & 0x0F);
            righthand = (byte) ((packed1 >> 4) & 0x0F);
            body = (byte) (packed2 & 0x0F);
            leg = (byte) ((packed2 >> 4) & 0x0F);
            shoes = (byte) (packed3 & 0x0F);
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            var packed1 = (byte) (lefthand | (righthand << 4));
            var packed2 = (byte) (body | (leg << 4));
            var packed3 = (byte) (shoes & 0x0F);

            Write(packed1);
            Write(packed2);
            Write(packed3);
        }
    }
}
