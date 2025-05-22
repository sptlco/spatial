// Copyright © Spatial. All rights reserved.

using Ignite.Models;
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
    /// Create a new <see cref="PROTO_EQUIPMENT"/>.
    /// </summary>
    public PROTO_EQUIPMENT() { }

    /// <summary>
    /// Create a new <see cref="PROTO_EQUIPMENT"/>.
    /// </summary>
    /// <param name="character">A <see cref="Character"/> to serialize.</param>
    public PROTO_EQUIPMENT(Character character)
    {
        ushort GetItemId(ItemEquipEnum equipment) => character.Equipment[(byte) equipment]?.ItemId ?? ushort.MaxValue;
        byte GetLevel(ItemEquipEnum equipment) => character.Equipment[(byte) equipment]?.Upgrades ?? 0;

        Equ_Head = GetItemId(ItemEquipEnum.ITEMEQUIP_HAT);
        Equ_Mouth = GetItemId(ItemEquipEnum.ITEMEQUIP_MOUTH);
        Equ_RightHand = GetItemId(ItemEquipEnum.ITEMEQUIP_RIGHTHAND);
        Equ_Body = GetItemId(ItemEquipEnum.ITEMEQUIP_BODY);
        Equ_LeftHand = GetItemId(ItemEquipEnum.ITEMEQUIP_LEFTHAND);
        Equ_Pant = GetItemId(ItemEquipEnum.ITEMEQUIP_LEG);
        Equ_Boot = GetItemId(ItemEquipEnum.ITEMEQUIP_SHOES);
        Equ_AccBoot = GetItemId(ItemEquipEnum.ITEMEQUIP_SHOESACC);
        Equ_AccPant = GetItemId(ItemEquipEnum.ITEMEQUIP_LEGACC);
        Equ_AccBody = GetItemId(ItemEquipEnum.ITEMEQUIP_BODYACC);
        Equ_AccHeadA = GetItemId(ItemEquipEnum.ITEMEQUIP_HATACC);
        Equ_MiniMon_R = GetItemId(ItemEquipEnum.ITEMEQUIP_MINIMON_R);
        Equ_Eye = GetItemId(ItemEquipEnum.ITEMEQUIP_EYE);
        Equ_AccLeftHand = GetItemId(ItemEquipEnum.ITEMEQUIP_LEFTHANDACC);
        Equ_AccRightHand = GetItemId(ItemEquipEnum.ITEMEQUIP_RIGHTHANDACC);
        Equ_AccBack = GetItemId(ItemEquipEnum.ITEMEQUIP_BACK);
        Equ_CosEff = GetItemId(ItemEquipEnum.ITEMEQUIP_COSEFF);
        Equ_AccHip = GetItemId(ItemEquipEnum.ITEMEQUIP_TAIL);
        Equ_Minimon = GetItemId(ItemEquipEnum.ITEMEQUIP_MINIMON);
        Equ_AccShield = GetItemId(ItemEquipEnum.ITEMEQUIP_SHIELDACC);
        upgrade = new PROTO_UPGRADES {
            lefthand = GetLevel(ItemEquipEnum.ITEMEQUIP_LEFTHAND),
            righthand = GetLevel(ItemEquipEnum.ITEMEQUIP_RIGHTHAND),
            body = GetLevel(ItemEquipEnum.ITEMEQUIP_BODY),
            leg = GetLevel(ItemEquipEnum.ITEMEQUIP_LEG),
            shoes = GetLevel(ItemEquipEnum.ITEMEQUIP_SHOES)
        };
    }

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
