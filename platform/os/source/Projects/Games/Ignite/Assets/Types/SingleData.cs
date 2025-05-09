// Copyright © Spatial. All rights reserved.

using System.Collections.Concurrent;

namespace Ignite.Assets.Types;

/// <summary>
/// A data entry.
/// /// </summary>
[Name("SingleData.shn")]
public class SingleData
{
	private static readonly ConcurrentDictionary<string, ushort> _cache = [];

	/// <summary>
	/// A crusader's base maximum light points.
	/// </summary>
	public static float MaxLP => GetValue("SenMaxLP");

	/// <summary>
	/// The index of the data entry.
	/// /// </summary>
	public string SingleDataIDX { get; set; }

	/// <summary>
	/// The value of the data entry.
	/// /// </summary>
	public ushort SingleDataValue { get; set; }

	/// <summary>
	/// Get the value of a data entry by its index.
	/// </summary>
	/// <param name="index">The index of the data entry.</param>
	/// <returns>The value of the data entry.</returns>
	private static ushort GetValue(string index)
	{
		return _cache.GetOrAdd(index, Asset.First<SingleData>("SingleData.shn", s => s.SingleDataIDX == index).SingleDataValue);
	}
}
