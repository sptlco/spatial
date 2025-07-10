// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Spatial.Networking;

namespace Spatial;

/// <summary>
/// A program bootstrapped with platform features.
/// </summary>
public class Application
{
    private WebApplication _api;
    private Time _time;

    /// <summary>
    /// Create a new <see cref="Application"/>.
    /// </summary>
    public Application()
    {
        _time = Time.Now;
    }

    /// <summary>
    /// The application's local time.
    /// </summary>
    protected Time Time => _time;

    /// <summary>
    /// Run an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Application"/> to run.</typeparam>
    /// <param name="args">Optional command-line arguments.</param>
    public static void Run<T>(params string[] args) where T : Application, new()
    {
        Run<T>(default, default, args);
    }

    /// <summary>
    /// Run an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Application"/> to run.</typeparam>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/>.</param>
    /// <param name="args">Optional command-line arguments.</param>
    public static void Run<T>(
        CancellationToken cancellationToken = default,
        params string[] args) where T : Application, new()
    {
        Run<T>(default, cancellationToken, args);
    }

    /// <summary>
    /// Run an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Application"/> to run.</typeparam>
    /// <param name="tickRate">The application's tick rate.</param>
    /// <param name="args">Optional command-line arguments.</param>
    public static void Run<T>(
        Time tickRate,
        params string[] args) where T : Application, new()
    {
        Run<T>(tickRate, default, args);
    }

    /// <summary>
    /// Run an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Application"/> to run.</typeparam>
    /// <param name="tickRate">The application's tick rate.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/>.</param>
    /// <param name="args">Optional command-line arguments.</param>
    public static void Run<T>(
        Time tickRate = default,
        CancellationToken cancellationToken = default,
        params string[] args) where T : Application, new()
    {
        // Configure the Telemetry pipeline.
        // Spatial uses Serilog under the hood for logging purposes.

        ConfigureTelemetry();

        var application = new T {

            // Spatial applications are configured with a web API.
            // Here, we create the web API and assign it to our application.

            _api = CreateWebApplication(args)
        };

        application._api.Start();

        if (cancellationToken == default)
        {
            cancellationToken = Environment.CancellationToken;
        }

        // Support both capped and uncapped tick rates.
        // If the tick rate parameter is passed, use that.

        if (tickRate > Time.Zero)
        {
            Ticker.Run(application.Tick, tickRate, cancellationToken);
        }
        else
        {
            Ticker.Run(application.Tick, cancellationToken);
        }

        application.Shutdown();
    }

    /// <summary>
    /// Start the <see cref="Application"/>.
    /// </summary>
    /// <param name="args">Optional command-line arguments.</param>
    public virtual void Start(params string[] args) { }

    /// <summary>
    /// Update the <see cref="Application"/>.
    /// </summary>
    /// <param name="delta"></param>
    public virtual void Update(Time delta) { }

    /// <summary>
    /// Shutdown the <see cref="Application"/>.
    /// </summary>
    public virtual void Shutdown() { }

    private static void ConfigureTelemetry()
    {
        var config = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
            .WriteTo.Console()
            .WriteTo.MongoDBCapped(
                databaseUrl: Environment.DatabaseConnectionString,
                collectionName: Constants.LogCollectionName)
            .WriteTo.File(
                path: Constants.LogFilePath,
                rollingInterval: RollingInterval.Infinite,
                rollOnFileSizeLimit: true);

#if DEBUG
        config.MinimumLevel.Is(LogEventLevel.Debug);
#endif

        Log.Logger = config.CreateLogger();
    }

    private static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSerilog();
        builder.Services.AddControllers();
        builder.Services.AddExceptionHandler<FaultHandler>();

        var application = builder.Build();

        if (!application.Environment.IsDevelopment())
        {
            application.UseHsts();
        }

        application.UseHttpsRedirection();
        application.UseStaticFiles();
        application.UseAuthorization();

        application.MapControllers();

        return application;
    }

    private void Tick(Time delta)
    {
        Server.Receive();

        Update(delta);

        Server.Send();

        _time += delta;
    }
}