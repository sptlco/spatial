// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Models;
using Serilog;
using Spatial.Networking;
using System;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> with augmented functionality.
/// </summary>
public class AugmentedController : Controller
{
    protected Session _session;

    /// <summary>
    /// Initialize the <see cref="Controller"/>.
    /// </summary>
    public override void Initialize(ushort command)
    {
        try
        {
            _session = Session.Find(_connection);

            Log.Debug("{user}: {controller}.{command}", _session.Account.Username, GetType().Name, (NETCOMMAND) command);
        }
        catch (InvalidOperationException)
        {
            Log.Debug("{controller}.{command} (authless)", GetType().Name, (NETCOMMAND) command);
        }
    }

    /// <summary>
    /// Monitor the connection.
    /// </summary>
    public AugmentedController Monitor()
    {
        _connection.Set(Properties.Monitor, true);
        _connection.Set(Properties.Alive, World.Time.Milliseconds);

        return this;
    }

    /// <summary>
    /// Authenticate the <see cref="Controller"/>.
    /// </summary>
    /// <param name="session">A <see cref="Session"/>.</param>
    public AugmentedController Authenticate(Session session)
    {
        _connection.Set(Properties.Session, (_session = session).Handle);
        return this;
    }

    /// <summary>
    /// Authenticate the <see cref="Controller"/>.
    /// </summary>
    /// <param name="handle">A <see cref="Session"/> handle.</param>
    public AugmentedController Authenticate(ushort handle)
    {
        return Authenticate(Session.Find(handle));
    }
}