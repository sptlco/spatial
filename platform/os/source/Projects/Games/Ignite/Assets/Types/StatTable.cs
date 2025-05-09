// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;
using Ignite.Models;

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
    /// <param name="character">The character's <see cref="Class"/>.</param>
    /// <param name="level">The character's level.</param>
    /// <returns>The character's stat points.</returns>
    public static byte Points(Class character, byte level)
    {
        var stats = Asset.First<StatTable>("World/ChrCommon.txt/StatTable", c => c.Level == level);

        return character switch
        {
            Class.Fighter => (byte) stats.Fig,
            Class.CleverFighter => (byte) stats.Cfig,
            Class.Warrior => (byte) stats.War,
            Class.Gladiator => (byte) stats.Gla,
            Class.Knight => (byte) stats.Kni,
            Class.Cleric => (byte) stats.Cle,
            Class.HighCleric => (byte) stats.Hcle,
            Class.Paladin => (byte) stats.Pal,
            Class.HolyKnight => (byte) stats.Hol,
            Class.Guardian => (byte) stats.Gua,
            Class.Archer => (byte) stats.Arc,
            Class.HawkArcher => (byte) stats.Harc,
            Class.Scout => (byte) stats.Sco,
            Class.SharpShooter => (byte) stats.Sha,
            Class.Ranger => (byte) stats.Ran,
            Class.Mage => (byte) stats.Mag,
            Class.WizMage => (byte) stats.Wmag,
            Class.Enchanter => (byte) stats.Enc,
            Class.Warlock => (byte) stats.Warl,
            Class.Wizard => (byte) stats.Wiz,
            Class.Trickster => (byte) stats.Jok,
            Class.Gambit => (byte) stats.Chs,
            Class.Renegade => (byte) stats.Cru,
            Class.Spectre => (byte) stats.Cls,
            Class.Reaper => (byte) stats.Ass,
            Class.Crusader => (byte) stats.Sen,
            Class.Templar => (byte) stats.Sav,
            _ => 0,
        };
    }
}
