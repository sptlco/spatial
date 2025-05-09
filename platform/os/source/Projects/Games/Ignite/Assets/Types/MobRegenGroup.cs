// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("MobRegen\\*.txt\\MobRegenGroup")]
public class MobRegenGroup
{
	private Area? _area;

	[JsonPropertyName("0")]
	public string GroupIndex { get; set; }

	[JsonPropertyName("1")]
	public string IsFamily { get; set; }

	[JsonPropertyName("2")]
	public int CenterX { get; set; }

	[JsonPropertyName("3")]
	public int CenterY { get; set; }

	[JsonPropertyName("4")]
	public int Width { get; set; }

	[JsonPropertyName("5")]
	public int Height { get; set; }

	[JsonPropertyName("6")]
	public int RangeDegree { get; set; }

	/// <summary>
	/// The group's <see cref="Assets.Area"/>.
	/// </summary>
	[JsonIgnore]
	public Area Area => _area ??= Locate();

	private Area Locate()
	{
		if (Height > 0)
		{
			return new Rectangle
			{
				Id = GroupIndex,
				Size = new(Width, Height),
				Position = new(CenterX, CenterY),
				Rotation = RangeDegree
			};
		}

		return new Circle
		{
			Id = GroupIndex,
			Position = new(CenterX, CenterY),
			Radius = RangeDegree
		};
	}
}
