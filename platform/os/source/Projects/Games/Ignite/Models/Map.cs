// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Components;
using Ignite.Contracts;
using Spatial.Simulation;
using Spatial.Structures;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
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
            var npc = map.Create(BodyType.NPC);

            map.Add(npc, new Template());
        }

        foreach (var mobRegenGroup in Asset.View<MobRegenGroup>($"MobRegen/{field.MapIDClient}.txt/MobRegenGroup"))
        {
            foreach (var mobRegen in Asset.View<MobRegen>($"MobRegen/{field.MapIDClient}.txt/MobRegen", mobRegen => mobRegen.RegenIndex == mobRegenGroup.GroupIndex))
            {
                for (var i = 0; i < mobRegen.MobNum; i++)
                {
                    var mob = map.Create(BodyType.Mob);

                    map.Add(mob, new Template());
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
    private readonly uint[] _entities = new uint[GetOffset(BodyType.Pet) + Constants.MaxObjects[BodyType.Pet]];
    private readonly ConcurrentDictionary<BodyType, InterlockedQueue<ushort>> _handles = [];
    private readonly ConcurrentDictionary<BodyType, StrongBox<uint>> _counters = [];

    /// <summary>
    /// Create a new <see cref="Entity"/>.
    /// </summary>
    /// <param name="type">The entity's <see cref="BodyType"/>.</param>
    /// <returns>An <see cref="Entity"/>.</returns>
    public Tag Create(BodyType type)
    {
        var tag = Provision(type);
        var entity = World.Space.Create(
            new Body(Type: type, Field: _data.Field.Serial, Map: _id),
            new Transform());

        _entities[tag] = entity;

        return tag;
    }

    /// <summary>
    /// Add a <see cref="IComponent"/> to a <see cref="Body"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to add.</typeparam>
    /// <param name="tag">The body's <see cref="Tag"/>.</param>
    public void Add<T>(in Tag tag) where T : unmanaged, IComponent
    {
        World.Space.Add<T>(_entities[tag]);
    }

    /// <summary>
    /// Add a <see cref="IComponent"/> to a <see cref="Body"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IComponent"/> to add.</typeparam>
    /// <param name="tag">The body's <see cref="Tag"/>.</param>
    /// <param name="component">A <see cref="IComponent"/>.</param>
    public void Add<T>(in Tag tag, in T component) where T : unmanaged, IComponent
    {
        World.Space.Add(_entities[tag], component);
    }

    /// <summary>
    /// Destroy a <see cref="Body"/>.
    /// </summary>
    /// <param name="tag">The body's <see cref="Tag"/>.</param>
    public void Destroy(in Tag tag)
    {
        World.Space.Destroy(_entities[tag]);

        _handles[tag.Decode().Type].Enqueue(tag);
    }

    private ushort Provision(BodyType type)
    {
        if (!_handles.GetOrAdd(type, _ => new()).TryDequeue(out var handle))
        {
            if (_counters.GetOrAdd(type, _ => new(0)).Value >= Constants.MaxObjects[type])
            {
                throw new InvalidOperationException($"The capacity for type {type} has been reached.");
            }

            handle = (ushort) (Interlocked.Increment(ref _counters[type].Value) - 1 + GetOffset(type));
        }

        return handle;
    }

    private static ushort GetOffset(BodyType type)
    {
        return type switch {
            BodyType.Mob => 0,
            BodyType.Player => 8000,
            BodyType.House => 9500,
            BodyType.Drop => 10500,
            BodyType.Chunk => 13500,
            BodyType.NPC => 17084,
            BodyType.Bandit => 17340,
            BodyType.Effect => 19388,
            BodyType.MagicField => 20388,
            BodyType.Door => 20638,
            BodyType.Servant => 21638,
            BodyType.Mount => 22138,
            BodyType.Pet => 23638,
            _ => throw new InvalidOperationException("The object type is unsupported.")
        };
    }
}
#endregion

/// <summary>
/// Mutate a <see cref="Map"/>.
/// </summary>
/// <param name="future">The future state of the <see cref="Map"/>.</param>
/// <param name="entity">An <see cref="Entity"/>.</param>
public delegate void Mutation(Future future, in Entity entity);