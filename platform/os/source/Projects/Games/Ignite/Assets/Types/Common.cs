// Copyright © Spatial. All rights reserved.

using Spatial.Simulation;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ChrCommon.txt\\Common")]
public class Common
{
	private static readonly ConcurrentDictionary<string, int> _cache = [];

	/// <summary>
	/// A character's base run speed.
	/// </summary>
	public static float RunSpeed => GetArgument("RunSpeed");

	/// <summary>
	/// A character's base walk speed.
	/// </summary>
	public static float WalkSpeed => GetArgument("WalkSpeed");

	/// <summary>
	/// A character's base attack speed.
	/// </summary>
	public static float AttackSpeed => GetArgument("AttackSpeed");

	/// <summary>
	/// The level a character must be to shout a message.
	/// </summary>
	public static byte ShoutLevel => (byte) GetArgument("ShoutLevel");

	/// <summary>
	/// A <see cref="Time"/> delay applied after shouting.
	/// </summary>
	public static Time ShoutDelay => Time.FromSeconds(GetArgument("ShoutDelay"));

	/// <summary>
	/// The maximum level of a character.
	/// </summary>
	public static byte LevelLimit => (byte) GetArgument("LevelLimit");

	/// <summary>
	/// The maximum bonus XP rate.
	/// </summary>
	public static float MaxBonusXP => GetArgument("MaxExpBonus") / 1000.0F;

	/// <summary>
	/// The XP rate applied to rested characters.
	/// </summary>
	public static float RestXP => GetArgument("RestExpRate") / 1000.0F;

	/// <summary>
	/// The level at which a character loses XP upon death.
	/// </summary>
	public byte XPLossLevel => (byte) GetArgument("LostExpLevel");

	/// <summary>
	/// The maximum amount of free stat points applied to a character.
	/// </summary>
	public byte MaxStatPoints => (byte) GetArgument("MaxFreeStat");

	[JsonPropertyName("0")]
	public string Index { get; set; }

	[JsonPropertyName("1")]
	public int Argument { get; set; }

	private static int GetArgument(string index)
	{
		return _cache.GetOrAdd(index, Asset.First<Common>("World/ChrCommon.txt/Common", parameter => parameter.Index.ToLower() == index.ToLower()).Argument);
	}
}
