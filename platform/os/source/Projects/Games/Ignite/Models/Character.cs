// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Contracts;
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
    /// The character's <see cref="Contracts.CharacterRace"/>.
    /// </summary>
    public CharacterRace Race { get; set; }

    /// <summary>
    /// The character's <see cref="Contracts.CharacterClass"/>.
    /// </summary>
    public CharacterClass Class { get; set; }

    /// <summary>
    /// The character's base <see cref="Contracts.CharacterClass"/>.
    /// </summary>
    public CharacterClass BaseClass => GetBaseClass();

    /// <summary>
    /// The character's <see cref="Contracts.Gender"/>.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// The character's <see cref="Models.Appearance"/>.
    /// </summary>
    public Appearance Shape { get; set; } = new();

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
    public Archetype Build { get; set; } = new();

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
    public GameOptions Options { get; set; } = new();

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
        CharacterRace race,
        CharacterClass @class,
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
            LP = (uint) (@class == CharacterClass.Crusader ? SingleData.MaxLP : 0),
            HPStones = (ushort) defaults.HPSoul,
            SPStones = (ushort) defaults.SPSoul,
            Money = (ulong) defaults.Money,
            Power = 100,
            Shape = new Appearance {
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

    private CharacterClass GetBaseClass()
    {
        return Class switch {
            >= CharacterClass.Crusader => CharacterClass.Crusader,
            >= CharacterClass.Trickster => CharacterClass.Trickster,
            >= CharacterClass.Mage => CharacterClass.Mage,
            >= CharacterClass.Archer => CharacterClass.Archer,
            >= CharacterClass.Cleric => CharacterClass.Cleric,
            _ => CharacterClass.Fighter
        };
    }

    /// <summary>
    /// The physical appearance of a <see cref="Character"/>.
    /// </summary>
    public class Appearance
    {
        /// <summary>
        /// The avatar's hair.
        /// </summary>
        public (byte Style, byte Color) Hair { get; set; }

        /// <summary>
        /// The avatar's face.
        /// </summary>
        public byte Face { get; set; }
    }

    /// <summary>
    /// A character's core stat distribution.
    /// </summary>
    public class Archetype
    {
        /// <summary>
        /// The character's strength points.
        /// </summary>
        public byte Strength { get; set; }

        /// <summary>
        /// The character's constitute points.
        /// </summary>
        public byte Endurance { get; set; }

        /// <summary>
        /// The character's dexterity points.
        /// </summary>
        public byte Dexterity { get; set; }

        /// <summary>
        /// The character's intelligence points.
        /// </summary>
        public byte Intelligence { get; set; }

        /// <summary>
        /// The character's wisdom points.
        /// </summary>
        public byte Wisdom { get; set; }

        /// <summary>
        /// The character's spirit points.
        /// </summary>
        public byte Spirit { get; set; }

        /// <summary>
        /// The character's stat point count.
        /// </summary>
        public byte Points { get; set; }
    }

    /// <summary>
    /// An character's active kingdom quest.
    /// </summary>
    public class KingdomQuest
    {
        /// <summary>
        /// The kingdom quest's handle.
        /// </summary>
        public uint Handle { get; set; }

        /// <summary>
        /// The map the <see cref="KingdomQuest"/> is taking place in.
        /// </summary>
        public string Map { get; set; }

        /// <summary>
        /// The character's position within the <see cref="KingdomQuest"/>.
        /// </summary>
        public Point2D Position { get; set; }

        /// <summary>
        /// The time the <see cref="KingdomQuest"/> began.
        /// </summary>
        public DateTime Time { get; set; }
    }

    /// <summary>
    /// Configurable options for a <see cref="Character"/>.
    /// </summary>
    public class GameOptions
    {
        /// <summary>
        /// The character's shortcut options.
        /// </summary>
        public byte[] Shortcuts { get; set; } = [];

        /// <summary>
        /// The character's shortcut size options.
        /// </summary>
        public byte[] ShortcutSize { get; set; } = [];

        /// <summary>
        /// The character's sound options.
        /// </summary>
        public byte[] Sound { get; set; } = [];

        /// <summary>
        /// The character's video options.
        /// </summary>
        public byte[] Video { get; set; } = [];

        /// <summary>
        /// The character's game options.
        /// </summary>
        public byte[] Game { get; set; } = [];

        /// <summary>
        /// The character's window options.
        /// </summary>
        public byte[] Windows { get; set; } = [];

        /// <summary>
        /// The character's key mappings.
        /// </summary>
        public byte[] Keys { get; set; } = [];
    }
}