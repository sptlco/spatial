// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Spatial.Extensions;
using Spatial.Mathematics;
using Spatial.Persistence;
using System;
using System.Collections.Generic;

namespace Ignite.Models;

/// <summary>
/// A persistent, player-owned entity.
/// </summary>
[DocumentCollection(Collection.Characters)]
public class Character : Document
{
    private readonly Inventory _inventory;
    private readonly Inventory _equipment;

    /// <summary>
    /// Create a new <see cref="Character"/>.
    /// </summary>
    public Character()
    {
        _inventory = new Inventory(InventoryType.Character, 192);
        _equipment = new Inventory(InventoryType.Equipment, 30);
    }

    /// <summary>
    /// The <see cref="Account"/> the <see cref="Character"/> belongs to.
    /// </summary>
    public uint Owner { get; set; }

    /// <summary>
    /// The character's slot, or positional index within the account's list 
    /// of characters (zero-based).
    /// </summary>
    public byte Slot { get; set; }

    /// <summary>
    /// The character's name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The character's <see cref="Models.Race"/>.
    /// </summary>
    public Race Race { get; set; }

    /// <summary>
    /// The character's <see cref="Models.Class"/>.
    /// </summary>
    public Class Class { get; set; }

    /// <summary>
    /// The character's base <see cref="Models.Class"/>.
    /// </summary>
    public Class BaseClass => GetBaseClass();

    /// <summary>
    /// The character's <see cref="Models.Gender"/>.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// The character's <see cref="Models.Appearance"/>.
    /// </summary>
    public Appearance Appearance { get; set; } = new();

    /// <summary>
    /// The character's level.
    /// </summary>
    public byte Level { get; set; } = 1;

    /// <summary>
    /// The character's experience points.
    /// </summary>
    public ulong XP { get; set; }

    /// <summary>
    /// The name of the map the <see cref="Character"/> is in.
    /// </summary>
    public byte Map { get; set; }

    /// <summary>
    /// The character's current position.
    /// /// </summary>
    public Point2D Position { get; set; }

    /// <summary>
    /// The character's current health points.
    /// /// </summary>
    public uint HP { get; set; }

    /// <summary>
    /// The character's current spirit points.
    /// /// </summary>
    public uint SP { get; set; }

    /// <summary>
    /// The character's current light points.
    /// </summary>
    public uint LP { get; set; }

    /// <summary>
    /// The character's statistical <see cref="Models.Archetype"/>.
    /// </summary>
    public Archetype Archetype { get; set; } = new();

    /// <summary>
    /// The character's current HP stone count.
    public ushort HPStones { get; set; }

    /// <summary>
    /// The character's current SP stone count.
    /// </summary>
    public ushort SPStones { get; set; }

    /// <summary>
    /// The character's <see cref="Inventory"/>.
    /// </summary>
    public Inventory Inventory => _inventory;

    /// <summary>
    /// The character's equipment <see cref="Inventory"/>.
    /// </summary>
    public Inventory Equipment => _equipment;

    /// <summary>
    /// The character's copper balance.
    /// </summary>
    public ulong Money { get; set; }

    /// <summary>
    /// The character's fame.
    /// </summary>
    public uint Fame { get; set; }

    /// <summary>
    /// The character's power level.
    /// </summary>
    public byte Power { get; set; }

    /// <summary>
    /// The character's prison sentence.
    /// </summary>
    public TimeSpan Sentence { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// The character's kill cooldown.
    /// </summary>
    public TimeSpan KillCooldown { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// The character's player kill count.
    /// </summary>
    public uint Kills { get; set; }

    /// <summary>
    /// The kingdom quest the <see cref="Character"/> is participating in.
    /// </summary>
    public KingdomQuest? KQ { get; set; }

    /// <summary>
    /// A list of requirements.
    /// </summary>
    public List<string> Requirements { get; set; } = [];

    /// <summary>
    /// Configurable options for the <see cref="Character"/>.
    /// </summary>
    public Options Options { get; set; } = new();

    /// <summary>
    /// The character's chat color.
    /// </summary>
    public (byte Font, byte Balloon) ChatColor { get; set; } = (0, 0);

    /// <summary>
    /// Create a new <see cref="Character"/>.
    /// </summary>
    /// <param name="account">The account the <see cref="Character"/> belongs to.</param>
    /// <param name="slot">The character's slot, or positional index within the account's list of characters (zero-based).</param>
    /// <param name="name">The character's name.</param>
    /// <param name="race">The character's <see cref="Race"/>.</param>
    /// <param name="class">The character's <see cref="Class"/>.</param>
    /// <param name="gender">The character's <see cref="Gender"/>.</param>
    /// <param name="hairStyle">The character's hair style.</param>
    /// <param name="hairColor">The character's hair color.</param>
    /// <param name="face">The character's face.</param>
    /// <returns>An <see cref="Character"/>.</returns>
    public static Character Create(
        uint account,
        byte slot,
        string name,
        Race race,
        Class @class,
        Gender gender,
        byte hairStyle,
        byte hairColor,
        byte face)
    {
        var defaults = Asset.First<CHARACTER>("DefaultCharacterData.txt/CHARACTER", c => c.Class == (int) @class);

        var character = new Character {
            Owner = account,
            Slot = slot,
            Name = name,
            Race = race,
            Class = @class,
            Gender = gender,
            Map = defaults.Map,
            Position = new(defaults.PX, defaults.PY),
            Level = (byte) defaults.InitLV,
            XP = (ulong) long.Parse(defaults.InitEXP),
            HP = (uint) defaults.HP,
            SP = (uint) defaults.SP,
            LP = (uint) (@class == Class.Crusader ? SingleData.MaxLP : 0),
            HPStones = (ushort) defaults.HPSoul,
            SPStones = (ushort) defaults.SPSoul,
            Money = (ulong) defaults.Money,
            Power = 100,
            Appearance = new Appearance {
                Hair = (hairStyle, hairColor),
                Face = face
            }
        };

        character.Store();

        var items = Asset.View<ITEM>("DefaultCharacterData.txt/ITEM", i => i.Class == (int) @class);
        var skills = Asset.View<SKILL>("DefaultCharacterData.txt/SKILL", s => s.Class == (int) @class);

        foreach (var data in items)
        {
            character.Inventory.Store(new Item {
                Owner = character.Id,
                ItemId = (ushort) data.ItemID,
                Lot = (ulong) data.Lot
            });
        }

        return character;
    }

    /// <summary>
    /// Load the <see cref="Character"/>.
    /// </summary>
    /// <returns>The loaded <see cref="Character"/>.</returns>
    public Character Load()
    {
        var items = Document<Item>.List(i => i.Owner == Id && i.Inventory == InventoryType.Character);
        var equipment = Document<Item>.List(i => i.Owner == Id && i.Inventory == InventoryType.Equipment);

        items.ForEach(item => _inventory[item.Slot] = item);
        equipment.ForEach(item => _equipment[item.Slot] = item);

        return this;
    }

    /// <summary>
    /// Delete the <see cref="Character"/>.
    /// </summary>
    public void Delete()
    {
        this.Remove();
    }

    private Class GetBaseClass()
    {
        return Class switch {
            >= Class.Crusader => Class.Crusader,
            >= Class.Trickster => Class.Trickster,
            >= Class.Mage => Class.Mage,
            >= Class.Archer => Class.Archer,
            >= Class.Cleric => Class.Cleric,
            _ => Class.Fighter
        };
    }
}