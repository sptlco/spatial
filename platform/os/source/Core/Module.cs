// Copyright © Spatial Corporation. All rights reserved.

using Serilog;
using Serilog.Events;
using Spatial.Compute;
using System.Runtime.CompilerServices;

namespace Spatial;

/// <summary>
/// The sole purpose of this class is to initialize Spatial, providing 
/// consumers of this library with a fully configured platform out of the box.
/// </summary>
internal class Module
{
    /// <summary>
    /// Initialize the <see cref="Module"/>.
    /// </summary>
    [ModuleInitializer]
    public static void Initialize()
    {
        AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
        Console.CancelKeyPress += (s, e) => Log.CloseAndFlush();

        var config = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                path: Constants.LogFilePath,
                rollingInterval: RollingInterval.Infinite,
                rollOnFileSizeLimit: true);

#if DEBUG
        config.MinimumLevel.Is(LogEventLevel.Debug);
#endif

        Log.Logger = config.CreateLogger();

        Processor.Run();
    }
}
