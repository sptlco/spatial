// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Exchange;
using Spatial.Networking;
using Spatial.Simulation;
using System.Collections.Concurrent;

namespace Spatial.Models;

/// <summary>
/// A digital representation of the physical world.
/// </summary>
public static class World
{
    private static readonly Space _space;
    private static Time _time;
    private static readonly ConcurrentDictionary<string, Time> _intervals;
    private static readonly Ethereum _ethereum;

    /// <summary>
    /// Create a new <see cref="World"/>.
    /// </summary>
    static World()
    {
        _space = Space.Empty();
        _intervals = [];
        _ethereum = new Ethereum();

        Sync();
    }

    /// <summary>
    /// Synchronize the <see cref="World"/>.
    /// </summary>
    public static void Sync()
    {
        _time = Time.Now;
    }

    /// <summary>
    /// Update the <see cref="World"/>.
    /// </summary>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public static void Update(Time delta)
    {
        Server.Receive();

        IntervalicInvoke(Time.FromSeconds(1), () => {
            // ...
        });

        _space.Update(delta);

        Server.Send();

        _time += delta;
    }

    private static void IntervalicInvoke(Time interval, Action function)
    {
        var key = string.Join('.', function.Method.DeclaringType?.Name, function.Method.Name);

        if (_intervals.GetOrAdd(key, _time) <= _time)
        {
            _intervals[key] += interval;
            function();
        }
    }
}