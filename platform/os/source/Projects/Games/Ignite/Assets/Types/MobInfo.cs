// Copyright © Spatial. All rights reserved.

using System.Collections.Concurrent;

namespace Ignite.Assets.Types;

[Name("MobInfo.shn")]
public class MobInfo
{
	private static readonly ConcurrentDictionary<string, (MobInfo, MobInfoServer)> _cache = [];
	private static readonly ConcurrentDictionary<ushort, (MobInfo, MobInfoServer)> _cacheById = [];

	public ushort ID { get; set; }

	public string InxName { get; set; }

	public string Name { get; set; }

	public uint Level { get; set; }

	public uint MaxHP { get; set; }

	public uint WalkSpeed { get; set; }

	public uint RunSpeed { get; set; }

	public byte IsNPC { get; set; }

	public uint Size { get; set; }

	public uint WeaponType { get; set; }

	public uint ArmorType { get; set; }

	public uint GradeType { get; set; }

	public uint Type { get; set; }

	public byte IsPlayerSide { get; set; }

	public uint AbsoluteSize { get; set; }

	/// <summary>
	/// The size of the mob's body.
	/// </summary>
	public float BodySize => Constants.UNIT * AbsoluteSize * Constants.SCALE;

	/// <summary>
	/// Load <see cref="MobInfo"/>.
	/// </summary>
	/// <param name="mobId">The mob's identification number.</param>
	/// <returns>The mob's data.</returns>
	public static (MobInfo Client, MobInfoServer Server) Load(ushort mobId)
	{
		return _cacheById.GetOrAdd(mobId, (Asset.First<MobInfo>("MobInfo.shn", mob => mob.ID == mobId), Asset.First<MobInfoServer>("MobInfoServer.shn", mob => mob.ID == mobId)));
	}

	/// <summary>
	/// Load <see cref="MobInfo"/>.
	/// </summary>
	/// <param name="mobName">The mob's name.</param>
	/// <returns>The mob's data.</returns>
	public static (MobInfo Client, MobInfoServer Server) Load(string mobName)
	{
		return _cache.GetOrAdd(mobName, (Asset.First<MobInfo>("MobInfo.shn", mob => mob.InxName == mobName), Asset.First<MobInfoServer>("MobInfoServer.shn", mob => mob.InxName == mobName)));
	}
}
