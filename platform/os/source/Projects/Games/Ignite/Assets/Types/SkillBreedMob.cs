// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\MiscDataTable.txt\\SkillBreedMob")]
public class SkillBreedMob
{
    [JsonPropertyName("0")]
    public byte Serial { get; set; }

    [JsonPropertyName("1")]
    public string Skill { get; set; }

    [JsonPropertyName("2")]
    public string MobIndex { get; set; }

    [JsonPropertyName("3")]
    public byte SummonNum { get; set; }

    [JsonPropertyName("4")]
    public string AI { get; set; }

    [JsonPropertyName("5")]
    public string RegenLoc { get; set; }

    [JsonPropertyName("6")]
    public short RegenDistance { get; set; }

    [JsonPropertyName("7")]
    public string MobRoam { get; set; }

    [JsonPropertyName("8")]
    public int LifeTime { get; set; }

    [JsonPropertyName("9")]
    public string AbState { get; set; }

    [JsonPropertyName("10")]
    public string TriggerObject { get; set; }

    [JsonPropertyName("11")]
    public int TriggerRange { get; set; }

    [JsonPropertyName("12")]
    public short Delay { get; set; }

    [JsonPropertyName("13")]
    public short ExplNo { get; set; }

    [JsonPropertyName("14")]
    public string Explosion { get; set; }

    [JsonPropertyName("15")]
    public string ExplosionWhere { get; set; }

    [JsonPropertyName("16")]
    public string Debuff { get; set; }

    [JsonPropertyName("17")]
    public string MultiTarget { get; set; }

    [JsonPropertyName("18")]
    public byte CheckSum { get; set; }
}