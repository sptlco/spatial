// Copyright © Spatial Corporation. All rights reserved.

using Serilog;
using Spatial.Compute;

namespace Spatial;

/// <summary>
/// A platform-powered environment.
/// </summary>
public static class Environment
{
    private static readonly CancellationTokenSource _cts;

    /// <summary>
    /// The system's private key.
    /// </summary>
    public static readonly string? PrivateKey = System.Environment.GetEnvironmentVariable("SPATIAL_PRIVATE_KEY");

    /// <summary>
    /// The system's RPC endpoint.
    /// </summary>
    public static readonly string? RPCUrl = System.Environment.GetEnvironmentVariable("SPATIAL_INFURA_URL");

    /// <summary>
    /// Create a new <see cref="Environment"/>.
    /// </summary>
    static Environment()
    {
        _cts = new();

        AppDomain.CurrentDomain.ProcessExit += (s, e) => Shutdown();
        Console.CancelKeyPress += (s, e) => Shutdown();
    }

    /// <summary>
    /// A <see cref="CancellationToken"/> that is cancelled when the 
    /// environment exits.
    /// </summary>
    public static CancellationToken CancellationToken => _cts.Token;

    private static void Shutdown()
    {
        _cts.Cancel();

        Processor.Shutdown();
        Log.CloseAndFlush();
    }
}
