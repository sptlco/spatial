// Copyright © Spatial. All rights reserved.

using Ignite.Models.Objects;
using Serilog;
using Spatial.Mathematics;
using Spatial.Networking;
using Spatial.Structures;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Ignite.Models;

/// <summary>
/// An active <see cref="Connection"/> to the <see cref="World"/>.
/// </summary>
public sealed class Session
{
    private static readonly SparseArray<Session> _sessions = new(Constants.Capacity);
    private static readonly ConcurrentBag<ushort> _pool = [];
    private static ushort _next;
    private static int _count;

    private readonly ushort _handle;
    private readonly Account _account;
    private readonly SparseArray<MenuFunction> _callbacks;
    private int _refs;


    private Session(ushort handle, Account account)
    {
        _handle = handle;
        _account = account;
        _callbacks = new SparseArray<MenuFunction>(10);
    }

    /// <summary>
    /// The session's handle.
    /// </summary>
    public ushort Handle => _handle;

    /// <summary>
    /// The session's <see cref="Models.Account"/>.
    /// </summary>
    public Account Account => _account;

    /// <summary>
    /// The session's one-time password.
    /// </summary>
    public Password? Password { get; set; }

    /// <summary>
    /// The session's <see cref="Models.World"/> <see cref="Connection"/>.
    /// </summary>
    public Connection World { get; set; }

    /// <summary>
    /// The session's <see cref="Models.Map"/> <see cref="Connection"/>.
    /// </summary>
    public Connection Map { get; set; }

    /// <summary>
    /// The session's <see cref="Models.Character"/>.
    /// </summary>
    public Character Character { get; set; }

    /// <summary>
    /// The session's <see cref="Models.Object"/>.
    /// </summary>
    public PlayerRef Object { get; set; }

    /// <summary>
    /// The session's callback functions.
    /// </summary>
    public SparseArray<MenuFunction> Callbacks => _callbacks;

    /// <summary>
    /// Create a new <see cref="Session"/>.
    /// </summary>
    /// <returns>A <see cref="Session"/>.</returns>
    public static Session Create(Account account)
    {
        if (!_pool.TryTake(out var handle))
        {
            if (_next >= _sessions.Capacity)
            {
                throw new InvalidOperationException("No available session handles.");
            }

            handle = _next++;
        }

        var session = new Session(handle, account);

        Log.Information("User {user} logged in.", session.Account.Username);

        Interlocked.Increment(ref _count);

        return _sessions[handle] = session;
    }

    /// <summary>
    /// Prune a <see cref="Session"/>.
    /// </summary>
    public static void Prune(Session? session)
    {
        if (session is null)
        {
            return;
        }
        
        Prune(session.World);
        Prune(session.Map);
    }

    /// <summary>
    /// Find a <see cref="Session"/>.
    /// </summary>
    /// <param name="handle">A <see cref="Session"/> handle.</param>
    /// <returns>A <see cref="Session"/>.</returns>
    public static Session Find(ushort handle)
    {
        return _sessions.ElementAtOrDefault(handle) ?? throw new InvalidOperationException("The session does not exist.");
    }

    /// <summary>
    /// Find a <see cref="Session"/> by a <see cref="Connection"/>.
    /// </summary>
    /// <param name="connection">A <see cref="Connection"/>.</param>
    /// <returns>A <see cref="Session"/>.</returns>
    public static Session Find(Connection connection)
    {
        if (!connection.Metadata.TryGetValue(Properties.Session, out var handle))
        {
            throw new InvalidOperationException("The session does not exist.");
        }
        
        return connection.Project<ushort, Session>(Properties.Session, Find);
    }

    /// <summary>
    /// Find a <see cref="Session"/> using a one-time password.
    /// </summary>
    /// <param name="otp">A one-time password.</param>
    /// <returns>A <see cref="Session"/>.</returns>
    public static Session? FindOrDefault(string otp)
    {
        for (var i = 0; i < _count; i++)
        {
            var session = _sessions.ElementAtOrDefault(i);
            var password = session?.Password;

            if (password is not null)
            {
                if (password.GUID == otp && DateTime.UtcNow < password.Expires)
                {
                    return session;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Decode a <see cref="Session"/> handle.
    /// </summary>
    /// <param name="key">A <see cref="Session"/> key.</param>
    /// <returns>A <see cref="Session"/> handle.</returns>
    public static ushort Decode(ushort[] key)
    {
        ushort handle = 0;

        for (var i = 0; i < key.Length; i++) 
        {
            handle ^= key[i];
        }

        return handle;
    }

    /// <summary>
    /// Count the number of active sessions.
    /// </summary>
    /// <returns>The current session count.</returns>
    public static int Count()
    {
        return _count;
    }

    /// <summary>
    /// Get a list of sessions.
    /// </summary>
    /// <returns></returns>
    public static Session?[] List() => _sessions.ToArray();

    /// <summary>
    /// Reference the <see cref="Session"/>.
    /// </summary>
    public Session Reference()
    {
        Interlocked.Increment(ref _refs);
        return this;
    }

    /// <summary>
    /// Generate a key for the <see cref="Session"/>.
    /// </summary>
    /// <returns>A key.</returns>
    public ushort[] GenerateKey()
    {
        var key = new ushort[32];
        
        key[^1] = _handle;
        
        for (var i = 0; i < key.Length - 1; i++)
        {
            key[^1] ^= key[i] = Strong.UInt16();
        }

        return key;
    }

    /// <summary>
    /// Generate a <see cref="Users.Password"/> for the <see cref="Session"/>.
    /// </summary>
    /// <returns>A <see cref="Users.Password"/>.</returns>
    public Password GenerateOTP()
    {
        return Password = new Password();
    }

    /// <summary>
    /// Release the <see cref="Session"/>.
    /// </summary>
    public void Release()
    {
        if (Interlocked.Decrement(ref _refs) == 0)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        Log.Information("User {user} logged out.", Account.Username);

        Object?.Release();
        Object = null!;

        _pool.Add(_handle);
        _sessions.Remove(_handle);

        Interlocked.Decrement(ref _count);
    }

    private static void Prune(Connection? connection)
    {
        if (connection != null && connection.Get<bool>(Properties.Monitor))
        {
            if (Models.World.Time - connection.Get<double>(Properties.Alive) >= Constants.MonitorThreshold)
            {
                connection.Disconnect();
            }
        }
    }
}

/// <summary>
/// A one-time password for a <see cref="Session"/>.
/// </summary>
public class Password
{
    /// <summary>
    /// A <see cref="Guid"/>.
    /// </summary>
    public string GUID { get; set; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// The password's expiration <see cref="DateTime"/>.
    /// </summary>
    public DateTime Expires { get; set; } = DateTime.UtcNow.Add(TimeSpan.FromSeconds(10));

    /// <summary>
    /// Convert the <see cref="Password"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="password">A <see cref="Password"/>.</param>
    public static implicit operator string(Password password) => password.GUID;
}