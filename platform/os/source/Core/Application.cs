// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Spatial.Networking;
using System.Net;

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

        INFO("Application starting...");

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

        INFO("Application running.");

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

        INFO("Application shutting down...");

        application.Shutdown();

        INFO("Application exited gracefully.");
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

        INFO("Application telemetry is currently enabled.");
    }

    private static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddSerilog()
            .AddExceptionHandler<FaultHandler>()
            .AddProblemDetails()
            .AddControllers();

        var application = builder.Build();

        application
            .UseExceptionHandler()
            .UseStatusCodePages(ReportStatusCode)
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseAuthorization()
            .UseHsts();

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

    private static async Task ReportStatusCode(StatusCodeContext status)
    {
        var context = status.HttpContext;
        var traceId = context.TraceIdentifier;

        switch ((HttpStatusCode) status.HttpContext.Response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                ERROR("Resource {Path} not found for request {Request}.", context.Request.Path, traceId);
                await context.Response.WriteAsJsonAsync(new NotFound().ToFault().ToResponse(traceId));
                break;
        }
    }
}