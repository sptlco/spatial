// Copyright © Spatial. All rights reserved.

using Spatial.Extensions;
using System.Text.Json.Serialization;

namespace Ignite.Assets.Types;

[Name("Script\\*.txt\\Script")]
public class Script
{
	[JsonPropertyName("0")]
	public string ScrIndex { get; set; }

	[JsonPropertyName("1")]
	public string ScrString { get; set; }

	/// <summary>
	/// Get the value of a <see cref="Script"/>.
	/// </summary>
	/// <param name="name">The script's name.</param>
	/// <param name="index">The script's index.</param>
	/// <param name="args">String arguments.</param>
	/// <returns>The script's value.</returns>
	public static string String(string name, string index, params object[] args)
	{
		return string.Format(Asset.First<Script>($"Script/{name}.txt/Script", s => s.ScrIndex == index).ScrString.ToPositionalFormat(), args);
	}
}
