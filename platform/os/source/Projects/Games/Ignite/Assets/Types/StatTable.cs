// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;
using Ignite.Contracts;

namespace Ignite.Assets.Types;

[Name("World\\ChrCommon.txt\\StatTable")]
public class StatTable
{
    [JsonPropertyName("0")]
    public byte Level { get; set; }

    [JsonPropertyName("1")]
    public string NextExp { get; set; }

    [JsonPropertyName("2")]
    public short Dummy { get; set; }

    [JsonPropertyName("3")]
    public short Fig { get; set; }

    [JsonPropertyName("4")]
    public short Cfig { get; set; }

    [JsonPropertyName("5")]
    public short War { get; set; }

    [JsonPropertyName("6")]
    public short Gla { get; set; }

    [JsonPropertyName("7")]
    public short Kni { get; set; }

    [JsonPropertyName("8")]
    public short Cle { get; set; }

    [JsonPropertyName("9")]
    public short Hcle { get; set; }

    [JsonPropertyName("10")]
    public short Pal { get; set; }

    [JsonPropertyName("11")]
    public short Hol { get; set; }

    [JsonPropertyName("12")]
    public short Gua { get; set; }

    [JsonPropertyName("13")]
    public short Arc { get; set; }

    [JsonPropertyName("14")]
    public short Harc { get; set; }

    [JsonPropertyName("15")]
    public short Sco { get; set; }

    [JsonPropertyName("16")]
    public short Sha { get; set; }

    [JsonPropertyName("17")]
    public short Ran { get; set; }

    [JsonPropertyName("18")]
    public short Mag { get; set; }

    [JsonPropertyName("19")]
    public short Wmag { get; set; }

    [JsonPropertyName("20")]
    public short Enc { get; set; }

    [JsonPropertyName("21")]
    public short Warl { get; set; }

    [JsonPropertyName("22")]
    public short Wiz { get; set; }

    [JsonPropertyName("23")]
    public short Jok { get; set; }

    [JsonPropertyName("24")]
    public short Chs { get; set; }

    [JsonPropertyName("25")]
    public short Cru { get; set; }

    [JsonPropertyName("26")]
    public short Cls { get; set; }

    [JsonPropertyName("27")]
    public short Ass { get; set; }

    [JsonPropertyName("28")]
    public short Sen { get; set; }

    [JsonPropertyName("29")]
    public short Sav { get; set; }

    [JsonPropertyName("30")]
    public int ExpLostAtPvP { get; set; }

    [JsonPropertyName("31")]
    public byte CheckSum { get; set; }

    /// <summary>
    /// Get the required XP for a level.
    /// </summary>
    /// <param name="level">The character's level.</param>
    /// <returns>The required XP for the level.</returns>
    public static ulong XP(byte level)
    {
        return ulong.Parse(Asset.FirstOrDefault<StatTable>("World/ChrCommon.txt/StatTable", c => c.Level == level - 1)?.NextExp ?? "0");
    }

    /// <summary>
    /// Get a character's stat points.
    /// </summary>
    /// <param name="character">The character's <see cref="CharacterClass"/>.</param>
    /// <param name="level">The character's level.</param>
    /// <returns>The character's stat points.</returns>
    public static byte Points(CharacterClass character, byte level)
    {
        var stats = Asset.First<StatTable>("World/ChrCommon.txt/StatTable", c => c.Level == level);

        return character switch
        {
            CharacterClass.Fighter => (byte) stats.Fig,
            CharacterClass.CleverFighter => (byte) stats.Cfig,
            CharacterClass.Warrior => (byte) stats.War,
            CharacterClass.Gladiator => (byte) stats.Gla,
            CharacterClass.Knight => (byte) stats.Kni,
            CharacterClass.Cleric => (byte) stats.Cle,
            CharacterClass.HighCleric => (byte) stats.Hcle,
            CharacterClass.Paladin => (byte) stats.Pal,
            CharacterClass.HolyKnight => (byte) stats.Hol,
            CharacterClass.Guardian => (byte) stats.Gua,
            CharacterClass.Archer => (byte) stats.Arc,
            CharacterClass.HawkArcher => (byte) stats.Harc,
            CharacterClass.Scout => (byte) stats.Sco,
            CharacterClass.SharpShooter => (byte) stats.Sha,
            CharacterClass.Ranger => (byte) stats.Ran,
            CharacterClass.Mage => (byte) stats.Mag,
            CharacterClass.WizMage => (byte) stats.Wmag,
            CharacterClass.Enchanter => (byte) stats.Enc,
            CharacterClass.Warlock => (byte) stats.Warl,
            CharacterClass.Wizard => (byte) stats.Wiz,
            CharacterClass.Trickster => (byte) stats.Jok,
            CharacterClass.Gambit => (byte) stats.Chs,
            CharacterClass.Renegade => (byte) stats.Cru,
            CharacterClass.Spectre => (byte) stats.Cls,
            CharacterClass.Reaper => (byte) stats.Ass,
            CharacterClass.Crusader => (byte) stats.Sen,
            CharacterClass.Templar => (byte) stats.Sav,
            _ => 0,
        };
    }
}
