// Copyright © Spatial. All rights reserved.

using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("World\\PineScript.txt\\PineScript")]
public class PineScript
{
	[JsonPropertyName("0")]
	public string ScriptName { get; set; }
}
