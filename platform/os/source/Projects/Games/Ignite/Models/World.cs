// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Spatial.Networking;
using Spatial.Simulation;
using Spatial.Structures;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Ignite.Models;

/// <summary>
/// The world of Isya.
/// </summary>
public static class World
{
    private static readonly Space _space;
    private static readonly ConcurrentDictionary<byte, SparseArray<Map>> _maps;
    private static readonly ConcurrentDictionary<string, Time> _intervals;
    private static Time _time;
    private static long _ticks;

    /// <summary>
    /// Create a new <see cref="World"/>.
    /// </summary>
    static World()
    {
        _space = Space.Empty();
        _maps = [];
        _intervals = [];

        Sync();
    }

    /// <summary>
    /// The world's underlying <see cref="Spatial.Simulation.Space"/>.
    /// </summary>
    public static Space Space => _space;

    /// <summary>
    /// The world's maps.
    /// </summary>
    public static ConcurrentDictionary<byte, SparseArray<Map>> Maps => _maps;

    /// <summary>
    /// The current status of the <see cref="World"/>.
    /// </summary>
    public static byte Status => GetStatus();

    /// <summary>
    /// The current <see cref="Spatial.Simulation.Time"/> in the world.
    /// </summary>
    public static Time Time => _time;

    /// <summary>
    /// The current tick count.
    /// </summary>
    public static long Ticks => _ticks;

    /// <summary>
    /// Synchronize the <see cref="World"/>.
    /// </summary>
    public static void Sync()
    {
        _time = Time.Now;
        _ticks = (long) (_time.Milliseconds * Constants.Delta);
    }

    /// <summary>
    /// Add a <see cref="Map"/> to the <see cref="World"/>.
    /// </summary>
    /// <param name="map">A <see cref="Map"/>.</param>
    public static void Add(Map map)
    {
        _maps[map.Serial][map.Id] = map;
    }

    /// <summary>
    /// Remove a <see cref="Map"/> from the <see cref="World"/>.
    /// </summary>
    /// <param name="map">A< <see cref="Map"/> serial number.</param>
    /// <param name="id">A <see cref="Map"/> identification number.</param>
    public static void Remove(byte map, int id)
    {
        _maps[map].Remove(id);
    }

    /// <summary>
    /// Update the <see cref="World"/>.
    /// </summary>
    /// <param name="delta"><see cref="Time"/> passed since the last update.</param>
    public static void Update(Time delta)
    {
        _space.Update(delta);

        _time += delta;
        _ticks += 1;
    }

    /// <summary>
    /// Issue a <see cref="NETCOMMAND"/> to a <see cref="Connection"/>.
    /// </summary>
    /// <param name="connection">A <see cref="Connection"/>.</param>
    /// <param name="command">The <see cref="NETCOMMAND"/> to issue.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after issuing the <see cref="NETCOMMAND"/>.</param>
    public static void Command(Connection connection, NETCOMMAND command, ProtocolBuffer data, bool dispose = true)
    {
        Server.Command(connection, (ushort) command, data, dispose);
    }

    /// <summary>
    /// Multicast a <see cref="ProtocolBuffer"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to multicast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="filter">An optional filter.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/> after multicasting.</param>
    public static void Multicast(NETCOMMAND command, ProtocolBuffer data, Expression<Func<Connection, bool>>? filter = default, bool dispose = true)
    {
        Server.Multicast((ushort) command, data, filter, dispose);
    }

    /// <summary>
    /// Broadcast a <see cref="ProtocolBuffer"/>.
    /// </summary>
    /// <param name="command">The <see cref="NETCOMMAND"/> to multicast.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="dispose">Whether or not dispose of the <see cref="ProtocolBuffer"/> after broadcasting.</param>
    public static void Broadcast(NETCOMMAND command, ProtocolBuffer data, bool dispose = true)
    {
        Server.Broadcast((ushort) command, data, dispose);
    }

    /// <summary>
    /// Perform an <see cref="Action"/> periodically.
    /// </summary>
    /// <param name="key">The interval's unique key.</param>
    /// <param name="interval">How often the action is performed.</param>
    /// <param name="function">The <see cref="Action"/> to perform.</param>
    public static void Interval(string key, Time interval, Action function)
    {
        if (_intervals.GetOrAdd(key, _time) <= _time)
        {
            _intervals[key] += interval;
            function();
        }
    }

    private static byte GetStatus()
    {
        return (byte) Math.Round(Math.Clamp(6 + 4 * ((double) Session.Count() / Constants.Capacity), 6.0D, 10.0D));
    }
}
