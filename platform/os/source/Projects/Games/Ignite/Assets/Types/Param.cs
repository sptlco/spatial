// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;
using Ignite.Contracts;

namespace Ignite.Assets.Types;

[Name("World\\*.txt\\Param")]
public class Param
{
    [JsonPropertyName("0")]
    public int Level { get; set; }

    [JsonPropertyName("1")]
    public int Strength { get; set; }

    [JsonPropertyName("2")]
    public int Constitution { get; set; }

    [JsonPropertyName("3")]
    public int Intelligence { get; set; }

    [JsonPropertyName("4")]
    public int Wizdom { get; set; }

    [JsonPropertyName("5")]
    public int Dexterity { get; set; }

    [JsonPropertyName("6")]
    public int MentalPower { get; set; }

    [JsonPropertyName("7")]
    public int SoulHP { get; set; }

    [JsonPropertyName("8")]
    public int MAXSoulHP { get; set; }

    [JsonPropertyName("9")]
    public int PriceHPStone { get; set; }

    [JsonPropertyName("10")]
    public int SoulSP { get; set; }

    [JsonPropertyName("11")]
    public int MAXSoulSP { get; set; }

    [JsonPropertyName("12")]
    public int PriceSPStone { get; set; }

    [JsonPropertyName("13")]
    public int AtkPerAP { get; set; }

    [JsonPropertyName("14")]
    public int DmgPerAP { get; set; }

    [JsonPropertyName("15")]
    public int MaxPwrStone { get; set; }

    [JsonPropertyName("16")]
    public int NumPwrStone { get; set; }

    [JsonPropertyName("17")]
    public int PricePwrStone { get; set; }

    [JsonPropertyName("18")]
    public int PwrStoneWC { get; set; }

    [JsonPropertyName("19")]
    public int PwrStoneMA { get; set; }

    [JsonPropertyName("20")]
    public int MaxGrdStone { get; set; }

    [JsonPropertyName("21")]
    public int NumGrdStone { get; set; }

    [JsonPropertyName("22")]
    public int PriceGrdStone { get; set; }

    [JsonPropertyName("23")]
    public int GrdStoneAC { get; set; }

    [JsonPropertyName("24")]
    public int GrdStoneMR { get; set; }

    [JsonPropertyName("25")]
    public int PainRes { get; set; }

    [JsonPropertyName("26")]
    public int RestraintRes { get; set; }

    [JsonPropertyName("27")]
    public int CurseRes { get; set; }

    [JsonPropertyName("28")]
    public int ShockRes { get; set; }

    [JsonPropertyName("29")]
    public short MaxHP { get; set; }

    [JsonPropertyName("30")]
    public short MaxSP { get; set; }

    [JsonPropertyName("31")]
    public int CharTitlePt { get; set; }

    [JsonPropertyName("32")]
    public int SkillPwrPt { get; set; }

    [JsonPropertyName("33")]
    public short JobChangeDmgUp { get; set; }

    /// <summary>
    /// Get a character's parameters.
    /// </summary>
    /// <param name="character">The character's <see cref="CharacterClass"/>.</param>
    /// <param name="level">The character's level.</param>
    /// <returns>The character's parameters.</returns>
    public static Param Stats(CharacterClass character, byte level)
    {
        return Asset.First<Param>($"World/Param{character}Server.txt/Param", p => p.Level == level);
    }
}