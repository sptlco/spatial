// Copyright © Spatial. All rights reserved.

using Ignite.Assets.Types;
using Ignite.Contracts;
using Ignite.Models.Objects;
using Spatial.Persistence;
using Spatial.Simulation;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ignite.Models;

/// <summary>
/// A persistent, player-owned entity.
/// </summary>
[DocumentCollection(Collection.Items)]
public class Item : Document
{
    private (ItemInfo Client, ItemInfoServer Server)? _data;

    /// <summary>
    /// The entity that owns the <see cref="Item"/>.
    /// </summary>
    public uint Owner { get; set; }

    /// <summary>
    /// The item's unique key.
    /// </summary>
    public ulong Key { get; set; } = SHINE_ITEM_REGISTNUMBER.Generate();

    /// <summary>
    /// The item's <see cref="Item"/> identification number.
    /// </summary>
    public ushort ItemId { get; set; }

    /// <summary>
    /// The item's data.
    /// </summary>
    public (ItemInfo Client, ItemInfoServer Server) Data => _data ??= ItemInfo.Read(ItemId);

    /// <summary>
    /// The <see cref="InventoryType"/> the <see cref="Item"/> is stored in.
    /// </summary>
    public InventoryType Inventory { get; set; }

    /// <summary>
    /// The inventory slot the <see cref="Item"/> is stored in.
    /// </summary>
    public byte Slot { get; set; }

    /// <summary>
    /// The item's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The posessed amount of the <see cref="Item"/>.
    /// </summary>
    public ulong Lot { get; set; }

    /// <summary>
    /// The consumable amount of the <see cref="Item"/>.
    /// </summary>
    public uint Amount { get; set; }

    /// <summary>
    /// The item's upgrade count.
    /// </summary>
    public byte Upgrades { get; set; }

    /// <summary>
    /// The item's fortification count.
    /// </summary>
    public byte Fortifications { get; set; }

    /// <summary>
    /// The item's failed upgrade count.
    /// </summary>
    public byte FailedUpgrades { get; set; }

    /// <summary>
    /// The <see cref="Time"/> the <see cref="Item"/> expires.
    /// </summary>
    public double? Expires { get; set; }

    /// <summary>
    /// The <see cref="Time"/> the <see cref="Item"/> was charged.
    /// </summary>
    public double? Charged { get; set; }

    /// <summary>
    /// The <see cref="Time"/> the item becomes available for use.
    /// </summary>
    public double? Useable { get; set; }

    /// <summary>
    /// The number of times the <see cref="Item"/> has been reconfigured.
    /// </summary>
    public byte Reconfigurations { get; set; }

    /// <summary>
    /// Whether or not the <see cref="Item"/> is bound to its owner.
    /// </summary>
    public bool Bound { get; set; }

    /// <summary>
    /// The item's grade.
    /// </summary>
    public byte Grade { get; set; }

    /// <summary>
    /// The item's ride count.
    /// </summary>
    public ushort Rides { get; set; }

    /// <summary>
    /// Whether or not the <see cref="Item"/> is being ridden.
    /// </summary>
    public bool Riding { get; set; }

    /// <summary>
    /// The item's current hunger level.
    /// </summary>
    public ushort Hunger { get; set; }

    /// <summary>
    /// The item's current health.
    /// </summary>
    public double Health { get; set; }

    /// <summary>
    /// The item's contents.
    /// </summary>
    public ulong[] Contents { get; set; } = [];

    /// <summary>
    /// The item's card identification number.
    /// </summary>
    public ushort Card { get; set; }

    /// <summary>
    /// The item's card group number.
    /// </summary>
    public ushort Group { get; set; }

    /// <summary>
    /// The item's serial number.
    /// </summary>
    public uint Serial { get; set; }

    /// <summary>
    /// The item's star count.
    /// </summary>
    public byte Stars { get; set; }

    /// <summary>
    /// The item's monetary value.
    /// </summary>
    public ulong Value { get; set; }

    /// <summary>
    /// Whether or not the item has been placed.
    /// </summary>
    public bool Placed { get; set; }

    /// <summary>
    /// The pet's identification number.
    /// </summary>
    public ushort PetId { get; set; } = ushort.MaxValue;

    /// <summary>
    /// The pet's registered identification number.
    /// </summary>
    public uint PetRID { get; set; }

    /// <summary>
    /// Whether or not the pet is being summoned.
    /// </summary>
    public bool Summoning { get; set; }

    /// <summary>
    /// The item's furniture identification number.
    /// </summary>
    public ushort FurnitureId { get; set; } = ushort.MaxValue;

    /// <summary>
    /// The <see cref="Placement"/> of the furniture piece.
    /// </summary>
    public Placement Position { get; set; } = new();

    /// <summary>
    /// The <see cref="Time"/> the item breaks;
    /// </summary>
    public double? Breaks { get; set; }

    /// <summary>
    /// The item's kill count.
    /// </summary>
    public ConcurrentDictionary<ushort, uint> Kills { get; set; } = [];

    /// <summary>
    /// The item's title mob identification number.
    /// </summary>
    public ushort TitleMobId { get; set; } = ushort.MaxValue;

    /// <summary>
    /// The maximum number of sockets that can be applied to the item.
    /// </summary>
    public byte MaxSockets { get; set; }

    /// <summary>
    /// The item's sockets.
    /// </summary>
    public List<Socket> Sockets { get; set; } = [];

    /// <summary>
    /// The item's portals.
    /// </summary>
    public List<Portal> Portals { get; set; } = [];

    /// <summary>
    /// Configurable options for the <see cref="Item"/>.
    /// </summary>
    public Dictionary<Option, float> Options { get; set; } = [];

    /// <summary>
    /// The item's upgraded options.
    /// </summary>
    public Dictionary<Option, float> UpgradeOptions { get; set; } = [];

    /// <summary>
    /// Equip the <see cref="Item"/>.
    /// </summary>
    /// <param name="player">The <see cref="PlayerRef"/> equipping the <see cref="Item"/>.</param>
    public void Equip(PlayerRef player)
    {
        // ...
    }

    /// <summary>
    /// Unequip the <see cref="Item"/>.
    /// </summary>
    /// <param name="player">The <see cref="PlayerRef"/> unequipping the <see cref="Item"/>.</param>
    public void Unequip(PlayerRef player)
    {
        // ...
    }

    /// <summary>
    /// Use the <see cref="Item"/>.
    /// </summary>
    /// <param name="player">The <see cref="PlayerRef"/> using the <see cref="Item"/>.</param>
    public void Use(PlayerRef player)
    {
        // ...
    }

    /// <summary>
    /// The position of a piece of furniture.
    /// </summary>
    public class Placement
    {
        /// <summary>
        /// The piece's X-coordinate.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The piece's Y-coordinate.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// The piece's Z-coordinate.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// The rotation of the piece.
        /// </summary>
        public float Rotation { get; set; }
    }

    /// <summary>
    /// An enchanted gem socket.
    /// </summary>
    public class Socket
    {
        /// <summary>
        /// The gem in the <see cref="Socket"/>.
        /// </summary>
        public ushort GemId { get; set; }

        /// <summary>
        /// The socket's rest count.
        /// </summary>
        public byte Rest { get; set; }
    }

    /// <summary>
    /// A map binding.
    /// </summary>
    public class Portal
    {
        /// <summary>
        /// The destination map.
        /// </summary>
        public ushort MapId { get; set; }

        /// <summary>
        /// The destination X-coordinate.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The destination Y-coordinate.
        /// </summary>
        public float Y { get; set; }
    }

    /// <summary>
    /// A configurable value.
    /// </summary>
    public enum Option
    {
        Strength = 0x1,
        Intelligence = 0x2,
        Dexterity = 0x3,
        Spirit = 0x4,
        Endurance = 0x5,
        Aim = 0x6,
        Evasion = 0x7,
        Defense = 0x8,
        Damage = 0x9,
        MagicDamage = 0xA,
        MagicDefense = 0xB
    }
}