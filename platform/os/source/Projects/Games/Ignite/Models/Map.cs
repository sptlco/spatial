// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Components;
using Ignite.Contracts;
using Ignite.Models.Objects;
using Ignite.Systems;
using Serilog;
using Spatial.Networking;
using Spatial.Simulation;
using Spatial.Structures;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
    private readonly Space _space;
    private readonly Grid _grid;

    private readonly Query _query;

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
        _space = Space.Empty();
        _grid = new(this);

        _query = new Query().Parallel(false).WithAll<Chunk>().WithNone<Disabled>();
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
    /// The map's <see cref="Spatial.Simulation.Space"/>.
    /// </summary>
    public Space Space => _space;

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
            (MobInfo Client, MobInfoServer Server) data = (
                Client: Asset.First<MobInfo>("MobInfo.shn", mob => mob.InxName == shineNpc.MobName),
                Server: Asset.First<MobInfoServer>("MobInfoServer.shn", mob => mob.InxName == shineNpc.MobName));

            var npc = map.CreateEntity(
                type: ObjectType.NPC,
                vitals: new Vitals(Level: (byte) data.Client.Level, Health: new Parameter(Math.Max(data.Client.MaxHP, 1)), Spirit: new Parameter(data.Server.MaxSP)),
                attributes: new Attributes(Strength: data.Server.Str, Endurance: data.Server.Con, Dexterity: data.Server.Dex, Intelligence: data.Server.Int, Wisdom: 0.0F, Spirit: data.Server.Men),
                transform: new Transform(X: shineNpc.CoordX, Y: shineNpc.CoordY, R: (shineNpc.Direct + 360) % 360),
                speed: new Speed(Walking: data.Client.WalkSpeed, Running: data.Client.RunSpeed, Rotation: data.Server.TurnSpeed));

            var role = Enum.Parse<NPCRole>(shineNpc.Role);
            var type = Enum.TryParse<NPCType>(shineNpc.RoleArg0, out var value) ? value : NPCType.Other;

            map.Space.Add(npc, new NPC(
                Id: data.Client.ID,
                Menu: shineNpc.NPCMenu != 0,
                Role: role,
                Type: type));

            if (role == NPCRole.Gate)
            {
                var gate = Asset.First<LinkTable>("World/NPC.txt/LinkTable", gate => gate.Argument == shineNpc.RoleArg0);
                var destination = Field.Find(gate.MapServer);

                map.Space.Add(npc, new Gate(Map: destination.Serial, Id: destination.Id, X: gate.CoordX, Y: gate.CoordY, R: (gate.Direct + 360) % 360));
                map.Space.Add(npc, new Collider(data.Client.AbsoluteSize));
            }
        }

        foreach (var mobRegenGroup in Asset.View<MobRegenGroup>($"MobRegen/{field.MapIDClient}.txt/MobRegenGroup"))
        {
            var position = new Transform(X: mobRegenGroup.Area.Position.X, Y: mobRegenGroup.Area.Position.Y);
            var region = map.Space.Create(
                position,
                new Region(
                    Shape: mobRegenGroup.Area is Circle ? Shape.Circle : Shape.Rectangle,
                    X: position.X,
                    Y: position.Y,
                    Width: (mobRegenGroup.Area as Rectangle)?.Size.X ?? 0,
                    Height: (mobRegenGroup.Area as Rectangle)?.Size.Y ?? 0,
                    Radius: (mobRegenGroup.Area as Circle)?.Radius ?? 0,
                    Rotation: (mobRegenGroup.Area as Rectangle)?.Rotation ?? 0));

            foreach (var mobRegen in Asset.View<MobRegen>($"MobRegen/{field.MapIDClient}.txt/MobRegen", mobRegen => mobRegen.RegenIndex == mobRegenGroup.GroupIndex))
            {
                var spawner = map.Space.Create(
                    new Spawner(),
                    new Transform(X: position.X, Y: position.Y));

                (MobInfo Client, MobInfoServer Server) data = (
                    Client: Asset.First<MobInfo>("MobInfo.shn", mob => mob.InxName == mobRegen.MobIndex),
                    Server: Asset.First<MobInfoServer>("MobInfoServer.shn", mob => mob.InxName == mobRegen.MobIndex));

                for (var i = 0; i < mobRegen.MobNum; i++)
                {
                    var mob = map.CreateEntity(
                        type: ObjectType.Mob,
                        vitals: new Vitals(Level: (byte) data.Client.Level, Health: new Parameter(data.Client.MaxHP), Spirit: new Parameter(data.Server.MaxSP)),
                        attributes: new Attributes(Strength: data.Server.Str, Endurance: data.Server.Con, Dexterity: data.Server.Dex, Intelligence: data.Server.Int, Spirit: data.Server.Men),
                        transform: Transform.Random(mobRegenGroup.Area),
                        speed: new Speed(Walking: data.Client.WalkSpeed, Running: data.Client.RunSpeed));

                    map.Space.Add(mob, new Mob(data.Client.ID));
                    map.Space.Add(mob, new Collider(data.Client.AbsoluteSize));

                    if (data.Server.EnemyDetectType != EnemyDetect.ED_NOBRAIN)
                    {
                        map.Space.Add(mob, new Intelligence());
                    }

                    map.Space.Join(mob, spawner);
                }

                map.Space.Join(spawner, region);
            }
        }

        map.Use<Clock>();
        map.Use<Generator>();
        map.Use<Computer>();
        map.Use<Brain>();

#if DEBUG
        map.Use<Debugger>();
#endif

        World.Add(map);

        return map;
    }

    /// <summary>
    /// Unload a <see cref="Map"/>.
    /// </summary>
    /// <param name="map">A <see cref="Map"/>.</param>
    public static void Unload(Map map)
    {
        World.Remove(map.Data.Field.Serial, map.Id);
        
        map.Space.Dispose();
    }

    /// <summary>
    /// Find a <see cref="Map"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="Map"/>.</param>
    /// <returns>A <see cref="Map"/>.</returns>
    public static Map InstanceAt(string name)
    {
        var map = Field.Find(name);

        return InstanceAt(map.Serial, map.Id);
    }

    /// <summary>
    /// Find a <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The map's serial number.</param>
    /// <param name="id">The map's identification number.</param>
    /// <returns>A <see cref="Map"/>.</returns>
    public static Map InstanceAt(byte map, int id = 0)
    {
        return InstanceAtOrDefault(map, id) ?? throw new InvalidOperationException("The map does not exist.");
    }

    /// <summary>
    /// Find a <see cref="Map"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="Map"/>.</param>
    /// <returns>A <see cref="Map"/>.</returns>
    public static Map? InstanceAtOrDefault(string name)
    {
        var map = Field.Find(name);

        return InstanceAtOrDefault(map.Serial, map.Id);
    }

    /// <summary>
    /// Find a <see cref="Map"/>.
    /// </summary>
    /// <param name="map">The map's serial number.</param>
    /// <param name="id">The map's identification number.</param>
    /// <returns>A <see cref="Map"/>.</returns>
    public static Map? InstanceAtOrDefault(byte map, int id = 0)
    {
        return World.Maps[map].ElementAtOrDefault(id);
    }

    /// <summary>
    /// Create a new plain <see cref="Object"/>.
    /// </summary>
    /// <param name="type">The <see cref="ObjectType"/> to create.</param>
    /// <returns>An <see cref="Object"/>.</returns>
    public T CreatePlainObject<T>(ObjectType type) where T : Object
    {
        return (T) CreatePlainObject(type);
    }

    /// <summary>
    /// Create a new <see cref="Object"/>.
    /// </summary>
    /// <param name="type">The <see cref="ObjectType"/> to create.</param>
    /// <param name="vitals">The object's <see cref="Vitals"/>.</param>
    /// <param name="attributes">The object's <see cref="Attributes"/>.</param>
    /// <param name="transform">The object's <see cref="Transform"/>.</param>
    /// <param name="speed">The object's <see cref="Speed"/>.</param>
    /// <returns>An <see cref="Object"/>.</returns>
    public T CreateObject<T>(
        ObjectType type,
        in Vitals? vitals = null,
        in Attributes? attributes = null,
        in Transform? transform = null,
        in Speed? speed = null) where T : Object
    {
        return (T) CreateObject(type, vitals, attributes, transform, speed);
    }

    /// <summary>
    /// Create a new plain <see cref="Object"/>.
    /// </summary>
    /// <param name="type">The <see cref="ObjectType"/> to create.</param>
    /// <returns>An <see cref="Object"/>.</returns>
    public Object CreatePlainObject(ObjectType type)
    {
        return ObjectAt(CreatePlainEntity(type));
    }

    /// <summary>
    /// Create a new <see cref="Object"/>.
    /// </summary>
    /// <param name="type">The <see cref="ObjectType"/> to create.</param>
    /// <param name="vitals">The object's <see cref="Vitals"/>.</param>
    /// <param name="attributes">The object's <see cref="Attributes"/>.</param>
    /// <param name="transform">The object's <see cref="Transform"/>.</param>
    /// <param name="speed">The object's <see cref="Speed"/>.</param>
    /// <returns>An <see cref="Object"/>.</returns>
    public Object CreateObject(
        ObjectType type,
        in Vitals? vitals = null,
        in Attributes? attributes = null,
        in Transform? transform = null,
        in Speed? speed = null)
    {
        return ObjectAt(CreateEntity(type, vitals, attributes, transform, speed));
    }

    /// <summary>
    /// Create a new plain <see cref="Object"/>.
    /// </summary>
    /// <param name="type">The <see cref="ObjectType"/> to create.</param>
    /// <returns>An <see cref="Entity"/>.</returns>
    public Entity CreatePlainEntity(ObjectType type)
    {
        return _space.Create(new Tag(Provision(type)));
    }

    /// <summary>
    /// Create a new <see cref="Object"/>.
    /// </summary>
    /// <param name="type">The <see cref="ObjectType"/> to create.</param>
    /// <param name="vitals">The object's <see cref="Vitals"/>.</param>
    /// <param name="attributes">The object's <see cref="Attributes"/>.</param>
    /// <param name="transform">The object's <see cref="Transform"/>.</param>
    /// <param name="speed">The object's <see cref="Speed"/>.</param>
    /// <returns>An <see cref="Entity"/>.</returns>
    public Entity CreateEntity(
        ObjectType type,
        in Vitals? vitals = null,
        in Attributes? attributes = null,
        in Transform? transform = null,
        in Speed? speed = null)
    {
        return _space.Create(
            new Tag(Provision(type)),
            vitals ?? new Vitals(),
            attributes ?? new Attributes(),
            transform ?? new Transform(_data.Info.RegenX, _data.Info.RegenY),
            speed ?? new Speed());
    }

    /// <summary>
    /// Reference an <see cref="Object"/>.
    /// </summary>
    /// <param name="handle">The object's handle.</param>
    /// <returns>An <see cref="Object"/>.</returns>
    public T ObjectAt<T>(ushort handle) where T : Object
    {
        return (T) ObjectAt(handle);
    }

    /// <summary>
    /// Reference an <see cref="Object"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> to reference.</param>
    /// <returns>An <see cref="Object"/>.</returns>
    public T ObjectAt<T>(Entity entity) where T : Object
    {
        return (T) ObjectAt(entity);
    }

    /// <summary>
    /// Reference an <see cref="Object"/>.
    /// </summary>
    /// <param name="handle">The object's handle.</param>
    /// <returns>An <see cref="Object"/>.</returns>
    public Object ObjectAt(ushort handle)
    {
        return ObjectAt(EntityAt(handle));
    }

    /// <summary>
    /// Reference an <see cref="Object"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Entity"/> to reference.</param>
    /// <returns>An <see cref="Object"/>.</returns>
    public Object ObjectAt(Entity entity)
    {
        return Object.Create(this, entity);
    }

    /// <summary>
    /// Get an <see cref="Entity"/> in the <see cref="Map"/>.
    /// </summary>
    /// <param name="handle">The object's handle.</param>
    /// <returns>An <see cref="Entity"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the <see cref="Entity"/> does not exist.</exception>
    public Entity EntityAt(ushort handle)
    {
        return _space.Query<Tag>(tag => tag.Handle == handle).First();
    }

    /// <summary>
    /// Query an area of the <see cref="Map"/>.
    /// </summary>
    /// <param name="type">The <see cref="ObjectType"/> to query.</param>
    /// <param name="x">The area's X-coordinate.</param>
    /// <param name="y">The area's Y-coordinate.</param>
    /// <param name="width">The width of the area.</param>
    /// <param name="height">The height of the area.</param>
    /// <param name="rotation">The area's rotation.</param>
    /// <returns>A list of entities matching the query.</returns>
    public IEnumerable<Entity> Query(ObjectType type, float x, float y, float width, float height, float rotation)
    {
        return Query(type, (entity) => {
            if (!_space.Has<Transform>(entity))
            {
                return false;
            }

            var transform = _space.Get<Transform>(entity);

            return Rectangle.Contains(x, y, width, height, rotation, transform.X, transform.Y);
        });
    }

    /// <summary>
    /// Query an area of the <see cref="Map"/>.
    /// </summary>
    /// <param name="type">The <see cref="ObjectType"/> to query.</param>
    /// <param name="x">The area's X-coordinate.</param>
    /// <param name="y">The area's Y-coordinate.</param>
    /// <param name="width">The width of the area.</param>
    /// <param name="height">The height of the area.</param>
    /// <param name="rotation">The area's rotation.</param>
    /// <returns>A list of entities matching the query.</returns>
    public IEnumerable<Entity> Query(ObjectType type, float x, float y, float radius)
    {
        return Query(type, (entity) => {
            if (!_space.Has<Transform>(entity))
            {
                return false;
            }

            var transform = _space.Get<Transform>(entity);

            return Circle.Contains(x, y, radius, transform.X, transform.Y);
        });
    }

    /// <summary>
    /// Query an area of the <see cref="Map"/>.
    /// </summary>
    /// <param name="types">The types of objects to query.</param>
    /// <param name="x">The area's X-coordinate.</param>
    /// <param name="y">The area's Y-coordinate.</param>
    /// <param name="width">The width of the area.</param>
    /// <param name="height">The height of the area.</param>
    /// <param name="rotation">The area's rotation.</param>
    /// <returns>A list of entities matching the query.</returns>
    public IEnumerable<Entity> Query(float x, float y, float width, float height, float rotation, params ObjectType[] types)
    {
        return Query(
            types: types,
            filter: (entity) => {
                if (!_space.Has<Transform>(entity))
                {
                    return false;
                }

                var transform = _space.Get<Transform>(entity);

                return Rectangle.Contains(x, y, width, height, rotation, transform.X, transform.Y);
            });
    }

    /// <summary>
    /// Query an area of the <see cref="Map"/>.
    /// </summary>
    /// <param name="types">The types of objects to query.</param>
    /// <param name="x">The area's X-coordinate.</param>
    /// <param name="y">The area's Y-coordinate.</param>
    /// <param name="width">The width of the area.</param>
    /// <param name="height">The height of the area.</param>
    /// <param name="rotation">The area's rotation.</param>
    /// <returns>A list of entities matching the query.</returns>
    public IEnumerable<Entity> Query(float x, float y, float radius, params ObjectType[] types)
    {
        return Query(
            types: types,
            filter: (entity) => {
                if (!_space.Has<Transform>(entity))
                {
                    return false;
                }

                var transform = _space.Get<Transform>(entity);

                return Circle.Contains(x, y, radius, transform.X, transform.Y);
            });
    }

    /// <summary>
    /// Query the <see cref="Map"/>.
    /// </summary>
    /// <param name="type">The <see cref="ObjectType"/> to query.</param>
    /// <returns>Objects of type <paramref name="type"/>.</returns>
    public IEnumerable<Entity> Query(ObjectType type, Func<Entity, bool>? filter = default)
    {
        return Query(new HashSet<ObjectType>([type]), filter);
    }

    /// <summary>
    /// Query the <see cref="Map"/>.
    /// </summary>
    /// <param name="types">The types to query.</param>
    /// <returns>Objects of type <paramref name="types"/>.</returns>
    public IEnumerable<Entity> Query(params ObjectType[] types)
    {
        return Query(new HashSet<ObjectType>(types));
    }

    /// <summary>
    /// Query the <see cref="Map"/>.
    /// </summary>
    /// <param name="types">The types to query.</param>
    /// <returns>Objects of type <paramref name="types"/>.</returns>
    public IEnumerable<Entity> Query(Func<Entity, bool>? filter, params ObjectType[] types)
    {
        return Query([.. types], filter);
    }

    /// <summary>
    /// Query the <see cref="Map"/>.
    /// </summary>
    /// <param name="types">The types to query.</param>
    /// <returns>Objects of type <paramref name="types"/>.</returns>
    public IEnumerable<Entity> Query(HashSet<ObjectType> types, Func<Entity, bool>? filter = default)
    {
        return _space.Query<Tag>((entity, tag) => (types.Count <= 0 || types.Contains(tag.Type)) && (filter?.Invoke(entity) ?? true));
    }

    /// <summary>
    /// Mutate the <see cref="Map"/>, dynamically updating active chunks.
    /// </summary>
    /// <param name="query">A <see cref="Spatial.Simulation.Query"/>.</param>
    /// <param name="mutation">A <see cref="Mutation"/>.</param>
    public void Dynamic(Query query, Mutation mutation)
    {
        _space.Mutate(_query, (Future future, in Entity entity) => Dynamic(future, entity, query, mutation));
    }

    /// <summary>
    /// Mutate the <see cref="Map"/>, dynamically updating active chunks.
    /// </summary>
    /// <param name="chunk">The <see cref="Chunk"/> to mutate.</param>
    /// <param name="query">A <see cref="Spatial.Simulation.Query"/> for the <see cref="Chunk"/>.</param>
    /// <param name="mutation">A <see cref="MutateFunction"/>.</param>
    public void Dynamic(Future future, Entity chunk, Query query, Mutation mutation)
    {
        _space.Mutate(
            query: query.WithAll<Transform>(),
            filter: (entity) => _grid.Contains(chunk, entity),
            function: (Future _, in Entity entity) => mutation(future, (chunk, _space.Get<Chunk>(chunk)), entity));
    }

    /// <summary>
    /// Broadcast a <see cref="NETCOMMAND"/> to the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to broadcast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after broadcasting.</param>
    public void Broadcast(NETCOMMAND command, ProtocolBuffer data, bool dispose = true)
    {
        Multicast(command, data, null, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a position in the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to broadcast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="position">The position to multicast to.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void Multicast2D(NETCOMMAND command, ProtocolBuffer data, in Transform position, bool dispose = true)
    {
        Multicast2D(command, data, position.X, position.Y, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a position in the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to broadcast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="position">The target X-coordinate.</param>
    /// <param name="position">The target Y-coordinate.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void Multicast2D(NETCOMMAND command, ProtocolBuffer data, in float x, in float y, bool dispose = true)
    {
        Multicast2D(command, data, x, y, _data.Info.Sight, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a <see cref="Circle"/> within the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to multicast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="position">The position of the <see cref="Circle"/>.</param>
    /// <param name="radius">The circle's radius.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void Multicast2D(NETCOMMAND command, ProtocolBuffer data, in Transform position, in float radius, bool dispose = true)
    {
        Multicast2D(command, data, position.X, position.Y, radius, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a <see cref="Circle"/> within the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to multicast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="x">The circle's X-coordinate.</param>
    /// <param name="y">The circle's Y-coordinate.</param>
    /// <param name="radius">The circle's radius.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void Multicast2D(NETCOMMAND command, ProtocolBuffer data, in float x, in float y, in float radius, bool dispose = true)
    {
        Multicast2DImpl(
            command: command,
            data: data,
            x: x,
            y: y,
            radius: radius,
            filter: null,
            dispose: dispose);
    }
    
    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a position in the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to broadcast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="position">The position to multicast to.</param>
    /// <param name="exclude">Excluded players.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void MulticastExclusive2D(NETCOMMAND command, ProtocolBuffer data, in Transform position, HashSet<ushort> exclude, bool dispose = true)
    {
        MulticastExclusive2D(command, data, position.X, position.Y, exclude, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a position in the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to broadcast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="x">The target X-coordinate.</param>
    /// <param name="y">The target Y-coordinate.</param>
    /// <param name="exclude">Excluded players.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void MulticastExclusive2D(NETCOMMAND command, ProtocolBuffer data, in float x, in float y, HashSet<ushort> exclude, bool dispose = true)
    {
        MulticastExclusive2D(command, data, x, y, _data.Info.Sight, exclude, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a <see cref="Circle"/> within the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to multicast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="position">The position of the <see cref="Circle"/>.</param>
    /// <param name="radius">The circle's radius.</param>
    /// <param name="exclude">Excluded players.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void MulticastExclusive2D(NETCOMMAND command, ProtocolBuffer data, in Transform position, in float radius, HashSet<ushort> exclude, bool dispose = true)
    {
        MulticastExclusive2D(command, data, position.X, position.Y, radius, exclude, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a <see cref="Circle"/> within the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to multicast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="x">The circle's X-coordinate.</param>
    /// <param name="y">The circle's Y-coordinate.</param>
    /// <param name="radius">The circle's radius.</param>
    /// <param name="exclude">Excluded players.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void MulticastExclusive2D(NETCOMMAND command, ProtocolBuffer data, in float x, in float y, in float radius, HashSet<ushort> exclude, bool dispose = true)
    {
        Multicast2DImpl(
            command: command,
            data: data,
            x: x,
            y: y,
            radius: radius,
            filter: (entity) => !exclude.Contains(_space.Get<Tag>(entity).Handle),
            dispose: dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a position in the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to broadcast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="position">The position to multicast to.</param>
    /// <param name="include">Included players.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void MulticastInclusive2D(NETCOMMAND command, ProtocolBuffer data, in Transform position, HashSet<ushort> include, bool dispose = true)
    {
        MulticastInclusive2D(command, data, position.X, position.Y, include, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a position in the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to broadcast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="x">The target X-coordinate.</param>
    /// <param name="y">The target Y-coordinate.</param>
    /// <param name="include">Included players.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void MulticastInclusive2D(NETCOMMAND command, ProtocolBuffer data, in float x, in float y, HashSet<ushort> include, bool dispose = true)
    {
        MulticastInclusive2D(command, data, x, y, _data.Info.Sight, include, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a <see cref="Circle"/> within the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to multicast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="position">The position of the <see cref="Circle"/>.</param>
    /// <param name="radius">The circle's radius.</param>
    /// <param name="include">Included players.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void MulticastInclusive2D(NETCOMMAND command, ProtocolBuffer data, in Transform position, in float radius, HashSet<ushort> include, bool dispose = true)
    {
        MulticastInclusive2D(command, data, position.X, position.Y, radius, include, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to a <see cref="Circle"/> within the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to multicast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="x">The circle's X-coordinate.</param>
    /// <param name="y">The circle's Y-coordinate.</param>
    /// <param name="radius">The circle's radius.</param>
    /// <param name="include">Included players.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void MulticastInclusive2D(NETCOMMAND command, ProtocolBuffer data, in float x, in float y, in float radius, HashSet<ushort> include, bool dispose = true)
    {
        Multicast2DImpl(
            command: command,
            data: data,
            x: x,
            y: y,
            radius: radius,
            filter: (entity) => include.Contains(_space.Get<Tag>(entity).Handle),
            dispose: dispose);
    }

    /// <summary>
    /// Multicast a <see cref="NETCOMMAND"/> to the <see cref="Map"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to multicast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="filter">An optional filter.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public void Multicast(NETCOMMAND command, ProtocolBuffer data, Func<Entity, bool>? filter = default, bool dispose = true)
    {
        data.Serialize(true);
        
        foreach (var entity in Query(ObjectType.Player, filter))
        {
            Server.Command(
                connection: Session.Find(_space.Get<Player>(entity).Session).Map,
                command: (ushort) command,
                data: data,
                dispose: false,
                serialize: false);
        }

        if (dispose)
        {
            data.Dispose();
        }                                                                                                                       
    }

    private void Multicast2DImpl(NETCOMMAND command, ProtocolBuffer data, in float x, in float y, in float radius, Func<Entity, bool>? filter = default, bool dispose = true)
    {
        data.Serialize(true);

        foreach (var entity in _grid.Query(x, y, radius, ObjectType.Player, filter))
        {
            Server.Command(
                connection: Session.Find(_space.Get<Player>(entity).Session).Map,
                command: (ushort) command,
                data: data,
                dispose: false,
                serialize: false);
        }

        if (dispose)
        {
            data.Dispose();
        }
    }

    private void Use<T>() where T : Systems.System
    {
        _space.Use<T>(this);
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

    /// <summary>
    /// Release an <see cref="Object"/>.
    /// </summary>
    /// <param name="object">An <see cref="Object"/>.</param>
    public void Release(Object @object)
    {
        var tag = @object.Tag;

        Release(tag.Type, tag.Handle);
    }

    /// <summary>
    /// Release an <see cref="Object"/>.
    /// </summary>
    /// <param name="entity">An <see cref="Entity"/>.</param>
    public void Release(Entity entity)
    {
        var tag = _space.Get<Tag>(entity);

        Release(tag.Type, tag.Handle);
    }

    /// <summary>
    /// Release an <see cref="Entity"/> handle.
    /// </summary>
    /// <param name="type">The <see cref="ObjectType"/> of handle to release.</param>
    /// <param name="handle">The handle to release.</param>
    public void Release(ObjectType type, ushort handle)
    {
        _handles.GetOrAdd(type, _ => new()).Enqueue(handle);
    }

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
/// Mutatea <see cref="Map"/>.
/// </summary>
/// <param name="future">The future state of the <see cref="Map"/>.</param>
/// <param name="entity">The target <see cref="Chunk"/>.</param>
/// <param name="entity">The target <see cref="Entity"/>.</param>
public delegate void Mutation(Future future, in (Entity Entity, Chunk Coordinates) chunk, in Entity entity);