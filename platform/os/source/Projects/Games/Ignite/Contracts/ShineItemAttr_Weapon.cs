// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_Weapon : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's upgrade level.
    /// </summary>
    public byte upgrade;

    /// <summary>
    /// The item's strength enhancements.
    /// </summary>
    public byte strengthen;

    /// <summary>
    /// The number of failed item upgrades.
    /// </summary>
    public byte upgradefailcount;

    /// <summary>
    /// Whether or not the item is bound to its owner.
    /// </summary>
    public SHINE_PUT_ON_BELONGED_ITEM IsPutOnBelonged;

    /// <summary>
    /// The monsters killed with the weapon.
    /// </summary>
    public Kill[] mobkills;

    /// <summary>
    /// The mob associated with the character's title.
    /// </summary>
    public ushort CharacterTitleMobID;

    /// <summary>
    /// The weapon's user title.
    /// </summary>
    public string usertitle;

    /// <summary>
    /// The weapon's sockets.
    /// </summary>
    public Socket[] gemSockets;

    /// <summary>
    /// The weapon's maximum socket count.
    /// </summary>
    public byte maxSocketCount;

    /// <summary>
    /// The weapon's created socket count.
    /// </summary>
    public byte createdSocketCount;

    /// <summary>
    /// The weapon's expiration date.
    /// </summary>
    public ShineDateTime deletetime;

    /// <summary>
    /// The number of times the weapon's random options have been changed.
    /// </summary>
    public byte randomOptionChangedCount;

    /// <summary>
    /// The weapon's options.
    /// </summary>
    public ItemOptionStorage option;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        upgrade = ReadByte();
        strengthen = ReadByte();
        upgradefailcount = ReadByte();
        IsPutOnBelonged = (SHINE_PUT_ON_BELONGED_ITEM) ReadInt32();
        mobkills = Read<Kill>(3);
        CharacterTitleMobID = ReadUInt16();
        usertitle = ReadString(21);
        gemSockets = Read<Socket>(3);
        maxSocketCount = ReadByte();
        createdSocketCount = ReadByte();
        deletetime = Read<ShineDateTime>();
        randomOptionChangedCount = ReadByte();
        option = Read<ItemOptionStorage>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(upgrade);
        Write(strengthen);
        Write(upgradefailcount);
        Write((int) IsPutOnBelonged);
        Write(mobkills);
        Write(CharacterTitleMobID);
        Write(usertitle, 21);
        Write(gemSockets);
        Write(maxSocketCount);
        Write(createdSocketCount);
        Write(deletetime);
        Write(randomOptionChangedCount);
        Write(option);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var kill in mobkills)
        {
            kill.Dispose();
        }

        foreach (var socket in gemSockets)
        {
            socket.Dispose();
        }

        deletetime.Dispose();
        option.Dispose();
        
        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing mob kill data.
    /// </summary>
    public class Kill : ProtocolBuffer
    {
        /// <summary>
        /// A null <see cref="Kill"/>.
        /// </summary>
        public static Kill Null = new Kill();

        /// <summary>
        /// The identification number of the monsters that was killed.
        /// </summary>
        public ushort monster = ushort.MaxValue;

        /// <summary>
        /// The number of monsters that were killed.
        /// </summary>
        public uint killcount = uint.MaxValue;

        /// <summary>
        /// Undocumented.
        /// </summary>
        public uint reserved;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            monster = ReadUInt16();

            var pack = ReadUInt32();

            killcount = pack & 0xFFFFFFF;
            reserved = (pack >> 28) & 0xF;
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(monster);
            Write(((reserved & 0xF) << 28) | (killcount & 0xFFFFFFF));
        }
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing socket data. 
    /// </summary>
    public class Socket : ProtocolBuffer
    {
        /// <summary>
        /// A null <see cref="Socket"/>.
        /// </summary>
        public static Socket Null = new Socket();

        /// <summary>
        /// The gem's identification number.
        /// </summary>
        public ushort elementalGemID = ushort.MaxValue;

        /// <summary>
        /// Undocumented.
        /// </summary>
        public byte restCount = byte.MaxValue;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            elementalGemID = ReadUInt16();
            restCount = ReadByte();
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(elementalGemID);
            Write(restCount);
        }
    }
}
