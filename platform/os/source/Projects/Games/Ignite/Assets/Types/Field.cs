// Copyright © Spatial. All rights reserved.

using Spatial.Mathematics;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

/// <summary>
/// A map.
/// </summary>
[Name("World\\Field.txt\\FieldList")]
public class Field
{
	private static readonly ConcurrentDictionary<byte, Field> _cache = [];

	private Point2D? _blocks;

	[JsonPropertyName("0")]
	public string MapIDClient { get; set; }

	[JsonPropertyName("1")]
	public string SubFrom { get; set; }

	[JsonPropertyName("2")]
	public string SubTo { get; set; }

	[JsonPropertyName("3")]
	public byte Serial { get; set; }

	[JsonPropertyName("4")]
	public string MapName { get; set; }

	[JsonPropertyName("5")]
	public FIELD_MAP_TYPE Type { get; set; }

	[JsonPropertyName("6")]
	public int Width { get; set; }

	[JsonPropertyName("7")]
	public int Height { get; set; }

	[JsonPropertyName("8")]
	public short ImmortalSec { get; set; }

	[JsonPropertyName("9")]
	public string ScriptName { get; set; }

	[JsonPropertyName("10")]
	public byte ItemDropByKilled { get; set; }

	[JsonPropertyName("11")]
	public byte Pker { get; set; }

	[JsonPropertyName("12")]
	public byte EnemyGuild { get; set; }

	[JsonPropertyName("13")]
	public byte Party { get; set; }

	[JsonPropertyName("14")]
	public byte Monster { get; set; }

	[JsonPropertyName("15")]
	public byte Summoned { get; set; }

	[JsonPropertyName("16")]
	public byte IsPKKQ { get; set; }

	[JsonPropertyName("17")]
	public byte IsFreePK { get; set; }

	[JsonPropertyName("18")]
	public byte IsPartyBattle { get; set; }

	[JsonPropertyName("19")]
	public byte NameHide { get; set; }

	[JsonPropertyName("20")]
	public byte LinkIn { get; set; }

	[JsonPropertyName("21")]
	public byte LinkOut { get; set; }

	[JsonPropertyName("22")]
	public byte SystemMap { get; set; }

	[JsonPropertyName("23")]
	public string RegenCity { get; set; }

	[JsonPropertyName("24")]
	public int RegenXA { get; set; }

	[JsonPropertyName("25")]
	public int RegenYA { get; set; }

	[JsonPropertyName("26")]
	public int RegenXB { get; set; }

	[JsonPropertyName("27")]
	public int RegenYB { get; set; }

	[JsonPropertyName("28")]
	public int RegenXC { get; set; }

	[JsonPropertyName("29")]
	public int RegenYC { get; set; }

	[JsonPropertyName("30")]
	public int RegenXD { get; set; }

	[JsonPropertyName("31")]
	public int RegenYD { get; set; }

	[JsonPropertyName("32")]
	public byte RegenSpot { get; set; }

	[JsonPropertyName("33")]
	public byte CanRestart { get; set; }

	[JsonPropertyName("34")]
	public byte CanTrade { get; set; }

	[JsonPropertyName("35")]
	public byte CanMiniHouse { get; set; }

	[JsonPropertyName("36")]
	public byte CanItem { get; set; }

	[JsonPropertyName("37")]
	public byte CanSkill { get; set; }

	[JsonPropertyName("38")]
	public byte Chat { get; set; }

	[JsonPropertyName("39")]
	public byte Shout { get; set; }

	[JsonPropertyName("40")]
	public byte CanBooth { get; set; }

	[JsonPropertyName("41")]
	public byte CanProduce { get; set; }

	[JsonPropertyName("42")]
	public byte CanRide { get; set; }

	[JsonPropertyName("43")]
	public byte CanStone { get; set; }

	[JsonPropertyName("44")]
	public byte CanParty { get; set; }

	[JsonPropertyName("45")]
	public short ExpLostAtDeadByMob { get; set; }

	[JsonPropertyName("46")]
	public short ExpLostAtDeadByPly { get; set; }

	[JsonPropertyName("47")]
	public byte UsrSubLayer { get; set; }

	[JsonPropertyName("48")]
	public byte CheckSum { get; set; }

	[JsonPropertyName("49")]
	public byte Zone { get; set; }

	/// <summary>
	/// The lower bound of the field's sub indices.
	/// </summary>
	public int From => int.TryParse(SubFrom, out var from) ? from : 0;

	/// <summary>
	/// The upper bound of the field's sub indices.
	/// </summary>
	public int To => int.TryParse(SubTo, out var to) ? to : 0;

	/// <summary>
	/// The field's block count.
	/// </summary>
	public Point2D Blocks => _blocks ??= new Point2D(Width * 8, Height * 8);

	/// <summary>
	/// Get a list of fields.
	/// </summary>
	/// <returns>A list of fields.</returns>
	public static IEnumerable<Field> List()
	{
		return Asset.View<Field>("World/Field.txt/FieldList");
	}

	/// <summary>
	/// Get a <see cref="Field"/> by its serial number.
	/// </summary>
	/// <param name="serial">A serial number.</param>
	/// <returns>A <see cref="Field"/>.</returns>
	public static Field Find(byte serial)
	{
		if (!_cache.TryGetValue(serial, out var field))
		{
			field = _cache[serial] = Asset.First<Field>("World/Field.txt/FieldList", f => f.Serial == serial);
		}

		return field;
	}

	/// <summary>
	/// Find a <see cref="Field"/> by name.
	/// </summary>
	/// <param name="name">The name of the <see cref="Field"/>.</param>
	/// <returns>The field's serial number and identification number.</returns>
	/// <exception cref="ArgumentException">Thrown if the field name is invalid.</exception>
	/// <exception cref="KeyNotFoundException">Thrown if the field does not exist.</exception>
	public static (byte Serial, int Id) Find(string name)
	{
		var match = Asset.FirstOrDefault<Field>("World/Field.txt/FieldList", f => f.MapIDClient == name);

		if (match != null)
		{
			return (match.Serial, 0);
		}

		for (var split = name.Length - 1; split >= 0; split--)
		{
			var prefix = name[..split];
			var suffix = name[split..];

			if (int.TryParse(suffix, out int instance))
			{
				var field = Asset.FirstOrDefault<Field>("World/Field.txt/FieldList", f => MatchesDeep(f, prefix, instance));

				if (field != null)
				{
					return (field.Serial, instance - int.Parse(field.SubFrom));
				}
			}
		}

		var fallback = Asset.FirstOrDefault<Field>("World/Field.txt/FieldList", f => MatchesShallow(f, name));

		if (fallback != null)
		{
			return (fallback.Serial, 0);
		}

		throw new KeyNotFoundException($"The field {name} does not exist.");
	}

	private static bool MatchesDeep(Field field, string name, int instance)
	{
		return field.MapIDClient == name &&
            int.TryParse(field.SubFrom, out int from) &&
            int.TryParse(field.SubTo, out int to) &&
            instance >= from &&
            instance <= to;
	}

	private static bool MatchesShallow(Field field, string name)
	{
		return field.MapIDClient == name &&
			field.SubFrom == "-" &&
			field.SubTo == "-";
	}
}
