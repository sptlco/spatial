// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\ItemUseFunction.txt\\ItemUseFunction")]
public class ItemUseFunction
{
	[JsonPropertyName("0")]
	public string ItemIndex { get; set; }

	[JsonPropertyName("1")]
	public byte BroadCast { get; set; }

	[JsonPropertyName("2")]
	public string UseFunction { get; set; }
}
