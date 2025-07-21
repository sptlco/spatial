// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Spatial.Blockchain;
using Spatial.Compute;
using Spatial.Networking;
using System.Net;

namespace Spatial;

/// <summary>
/// A program bootstrapped with platform features.
/// </summary>
public class Application
{
    private static Application _instance;

    private readonly Processor _processor;
    private readonly Network _network;
    private readonly Ethereum _ethereum;
    private readonly WebApplication _wapp;
    private Time _time;
    private long _ticks;

    /// <summary>
    /// Create a new <see cref="Application"/>.
    /// </summary>
    public Application()
    {
        _instance = this;
        _processor = new Processor();
        _network = new Network((_wapp = CreateWebApplication()).Services);
        _ticks = (_time = Time.Now).Ticks;
    }

    /// <summary>
    /// The currently running <see cref="Application"/>.
    /// </summary>
    public static Application Current => _instance;

    /// <summary>
    /// The application's <see cref="Spatial.Configuration"/>;
    /// </summary>
    public Configuration Configuration => _wapp.Services.GetRequiredService<IOptionsMonitor<Configuration>>().CurrentValue;

    /// <summary>
    /// The application's <see cref="Compute.Processor"/>.
    /// </summary>
    public Processor Processor => _processor;

    /// <summary>
    /// The application's <see cref="Networking.Network"/>.
    /// </summary>
    public Network Network => _network;

    /// <summary>
    /// The application's <see cref="Blockchain.Ethereum"/> provider.
    /// </summary>
    public Ethereum Ethereum => _ethereum;

    /// <summary>
    /// The application's local time.
    /// </summary>
    public Time Time => _time;

    /// <summary>
    /// The application's tick count.
    /// </summary>
    public long Ticks => _ticks;

    /// <summary>
    /// Run an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Application"/> to run.</typeparam>
    public static void Run<T>() where T : Application, new()
    {
        Run<T>(default, default);
    }

    /// <summary>
    /// Run an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Application"/> to run.</typeparam>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/>.</param>
    public static void Run<T>(CancellationToken cancellationToken) where T : Application, new()
    {
        Run<T>(default, cancellationToken);
    }

    /// <summary>
    /// Run an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Application"/> to run.</typeparam>
    /// <param name="tickRate">The application's tick rate.</param>
    public static void Run<T>(Time tickRate) where T : Application, new()
    {
        Run<T>(tickRate, default);
    }

    /// <summary>
    /// Run an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Application"/> to run.</typeparam>
    /// <param name="tickRate">The application's tick rate.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/>.</param>
    public static async void Run<T>(
        Time tickRate = default,
        CancellationToken cancellationToken = default) where T : Application, new()
    {
        var application = new T();

        ConfigureTelemetry();

        INFO("Application starting...");

        application.Start();
        application._wapp.Start();
        application._processor.Run();
        application._network.Open(application.Configuration.Network.Endpoint);

        if (cancellationToken == default)
        {
            cancellationToken = CreateCancellationToken();
        }

        INFO("Application running.");

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
        await application._wapp.StopAsync(CancellationToken.None);
        application._processor.Shutdown();
        application._network.Close();

        INFO("Application exited gracefully.");

        Log.CloseAndFlush();
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
                databaseUrl: Current.Configuration.Database.ConnectionString,
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

    private WebApplication CreateWebApplication()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services
            .AddOptions<Configuration>()
            .Bind(builder.Configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();

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

    private static CancellationToken CreateCancellationToken()
    {
        var source = new CancellationTokenSource();

        AppDomain.CurrentDomain.ProcessExit += (s, e) => source.Cancel();
        Console.CancelKeyPress += (s, e) => source.Cancel();

        return source.Token;
    }

    private async Task ReportStatusCode(StatusCodeContext status)
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

    private void Tick(Time delta)
    {
        _network.Receive();

        if (Configuration.Ethereum.Trades.Enabled)
        {
            Interval.Invoke(Trade, Time.FromMinutes(1));
        }

        Update(delta);

        _network.Send();

        _time += delta;
        _ticks++;
    }

    private void Trade()
    {
        // ...
    }
}