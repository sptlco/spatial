// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Contracts;
using Spatial.Simulation;
using Spatial.Structures;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Ignite.Models;

/// <summary>
/// An area of the <see cref="World"/> where entities exist and interact.
/// </summary>
public partial class Map
{
    private readonly (Field Field, Blockmap Blocks, MapInfo Info, MapViewInfo View) _data;

    private readonly int _id;
    private readonly string _name;
    private readonly Grid _grid;

    /// <summary>
    /// Create a new <see cref="Map"/>.
    /// </summary>
    /// <param name="field">The map's underlying <see cref="Field"/>.</param>
    /// <param name="id">The map's identification number.</param>
    public Map(Field field, int id)
    {
        _data = (
            Field: field,
            Blocks: Asset.Require<Blockmap>($"BlockInfo/{field.MapIDClient}.shbd"),
            Info: Asset.First<MapInfo>("MapInfo.shn", m => m.MapName == field.MapIDClient),
            View: Asset.First<MapViewInfo>("MapViewInfo.shn", m => m.MapName == field.MapIDClient));

        _id = id;
        _name = CreateName();
        _grid = new(this);
    }

    /// <summary>
    /// The map's underlying data.
    /// </summary>
    public (Field Field, Blockmap Blocks, MapInfo Info, MapViewInfo View) Data => _data;

    /// <summary>
    /// The map's serial number.
    /// </summary>
    public byte Serial => _data.Field.Serial;

    /// <summary>
    /// The map's identification number.
    /// </summary>
    public int Id => _id;

    /// <summary>
    /// The name of the <see cref="Map"/>.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// The map's <see cref="Models.Grid"/>.
    /// </summary>
    public Grid Grid => _grid;

    /// <summary>
    /// Load a <see cref="Map"/>.
    /// </summary>
    /// <param name="field">The map's <see cref="Field"/>.</param>
    /// <param name="id">The map's identification number.</param>
    /// <returns>A <see cref="Map"/>.</returns>
    public static Map Load(Field field, int id)
    {
        var map = new Map(field, id);

        foreach (var shineNpc in Asset.View<ShineNPC>("World/NPC.txt/ShineNPC", shineNpc => shineNpc.Map == field.MapIDClient))
        {
        }

        foreach (var mobRegenGroup in Asset.View<MobRegenGroup>($"MobRegen/{field.MapIDClient}.txt/MobRegenGroup"))
        {
            foreach (var mobRegen in Asset.View<MobRegen>($"MobRegen/{field.MapIDClient}.txt/MobRegen", mobRegen => mobRegen.RegenIndex == mobRegenGroup.GroupIndex))
            {
                for (var i = 0; i < mobRegen.MobNum; i++)
                {
                    
                }
            }
        }

        World.Add(map);

        return map;
    }

    private string CreateName()
    {
        string? suffix = null;

        if (int.TryParse(_data.Field.SubFrom, out var from))
        {
            suffix = (from + _id).ToString($"D{_data.Field.SubFrom.Length}");
        }

        return $"{_data.Field.MapIDClient}{suffix ?? ""}";
    }
}

#region Handle Allocation
public partial class Map
{
    private readonly ConcurrentDictionary<ObjectType, InterlockedQueue<ushort>> _handles = [];
    private readonly ConcurrentDictionary<ObjectType, ushort> _counters = [];
    private readonly Lock _lock = new();

    private ushort Provision(ObjectType type)
    {
        if (!_handles.GetOrAdd(type, _ => new()).TryDequeue(out var handle))
        {
            lock (_lock)
            {
                if (_counters.GetOrAdd(type, _ => 0) >= Constants.MaxObjects[type])
                {
                    throw new InvalidOperationException($"The capacity for type {type} has been reached.");
                }

                handle = _counters[type]++;

                handle += type switch {
                    ObjectType.Mob => 0,
                    ObjectType.Player => 8000,
                    ObjectType.House => 9500,
                    ObjectType.Drop => 10500,
                    ObjectType.Chunk => 13500,
                    ObjectType.NPC => 17084,
                    ObjectType.Bandit => 17340,
                    ObjectType.Effect => 19388,
                    ObjectType.MagicField => 20388,
                    ObjectType.Door => 20638,
                    ObjectType.Servant => 21638,
                    ObjectType.Mount => 22138,
                    ObjectType.Pet => 23638,
                    _ => throw new InvalidOperationException("The object type is unsupported.")
                };
            }
        }

        return handle;
    }
}
#endregion

/// <summary>
/// Mutate a <see cref="Map"/>.
/// </summary>
/// <param name="future">The future state of the <see cref="Map"/>.</param>
/// <param name="entity">An <see cref="Entity"/>.</param>
public delegate void Mutation(Future future, in Entity entity);