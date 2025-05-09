// Copyright © Spatial. All rights reserved.

using Ignite.Models;
using Spatial.Networking;
using System;
using System.Collections.Generic;

namespace Ignite;

/// <summary>
/// Constant values used in Ignite.
/// </summary>
public static class Constants
{
    /// <summary>
    /// The root directory.
    /// </summary>
    public static string Root = AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// Convert pixels to units.
    /// </summary>
    public const float U = 50.0F;

    /// <summary>
    /// Convert units to pixels.
    /// </summary>
    public const float px = 1 / U;

    /// <summary>
    /// The default port the server listens on.
    /// </summary>
    public const int DefaultServerPort = 9010;

    /// <summary>
    /// The server's tick rate (ticks per second).
    /// </summary>
    public const double TPS = 20.0D;

    /// <summary>
    /// The number of milliseconds per tick.
    /// </summary>
    public const double Delta = 1000.0D / TPS;

    /// <summary>
    /// Supported client versions.
    /// </summary>
    public static readonly string[] Versions = [
        "10022024000000"
    ];

    /// <summary>
    /// The capacity of the <see cref="World"/>.
    /// </summary>
    public const int Capacity = 2048;

    /// <summary>
    /// The name of the <see cref="World"/>.
    /// </summary>
    public const string WorldName = "Enid";

    /// <summary>
    /// The monitoring interval's key.
    /// </summary>
    public const string MonitorKey = "monitor";

    /// <summary>
    /// The console update key.
    /// </summary>
    public const string ConsoleKey = "console";

    /// <summary>
    /// The heartbeat monitoring interval in milliseconds.
    /// </summary>
    public const double MonitorInterval = 5000D;

    /// <summary>
    /// The heartbeat monitoring threshold before a connection is pronounced dead.
    /// </summary>
    public const double MonitorThreshold = MonitorInterval * 12.0D * 3.0D;

    /// <summary>
    /// The number of abnormal state bits.
    /// </summary>
    public const int AbnormalStateBits = 99;

    /// <summary>
    /// The maximum number of handles available for each <see cref="ObjectType"/>.
    /// </summary>
    public static readonly Dictionary<ObjectType, ushort> MaxObjects = new Dictionary<ObjectType, ushort> {
        [ObjectType.Mob] = 8000,
        [ObjectType.Player] = 1500,
        [ObjectType.House] = 1000,
        [ObjectType.Drop] = 3000,
        [ObjectType.Chunk] = 3584,
        [ObjectType.NPC] = 256,
        [ObjectType.Bandit] = 2048,
        [ObjectType.Effect] = 1000,
        [ObjectType.MagicField] = 250,
        [ObjectType.Door] = 1000,
        [ObjectType.Servant] = 500,
        [ObjectType.Mount] = 1000,
        [ObjectType.Pet] = 1500
    };

    /// <summary>
    /// The prefix for administrator commands.
    /// </summary>
    public const string CommandPrefix = "&";
}

/// <summary>
/// Constant collection names.
/// </summary>
public static class Collection
{
    /// <summary>
    /// The name of the <see cref="Account"/> database collection.
    /// </summary>
    public const string Accounts = "accounts";

    /// <summary>
    /// The name of the <see cref="Character"/> database collection.
    /// </summary>
    public const string Characters = "characters";
    
    /// <summary>
    /// The name of the <see cref="Item"/> collection.
    /// </summary>
    public const string Items = "items";

    /// <summary>
    /// The name of the <see cref="Skill"/> collection.
    /// </summary>
    public const string Skills = "skills";

    /// <summary>
    /// The name of the <see cref="Shortcut"/> collection.
    /// </summary>
    public const string Shortcuts = "shortcut";
}

public static class Requirements
{
    /// <summary>
    /// An <see cref="Character"/> requires a name change.
    /// </summary>
    public const string Name = "name";
}

/// <summary>
/// Constant properties for a <see cref="Connection"/>.
/// </summary>
public static class Properties
{
    /// <summary>
    /// The session property of a <see cref="Connection"/> indicates the 
    /// <see cref="Models.Session"/> associated with the <see cref="Connection"/>.
    /// </summary>
    public const string Session = "session";

    /// <summary>
    /// The monitor property of a <see cref="Connection"/> enables or 
    /// disables heartbeat monitoring.
    /// </summary>
    public const string Monitor = "monitor";

    /// <summary>
    /// The alive property of a <see cref="Connection"/> indicates the 
    /// last time the <see cref="Connection"/> was known to be alive.
    /// </summary>
    public const string Alive = "alive";
}