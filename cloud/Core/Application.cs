// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Spatial.Compute;
using Spatial.Extensions;
using Spatial.Networking;
using Spatial.Simulation;
using System.Net;
using System.Reflection;

namespace Spatial;

/// <summary>
/// A program bootstrapped with platform features.
/// </summary>
public class Application
{
    private static Application _instance;

    private readonly Space _space;
    private readonly WebApplication _wapp;
    private Time _time;
    private long _ticks;

    private readonly Processor _processor;
    private readonly Network _network;

    /// <summary>
    /// Create a new <see cref="Application"/>.
    /// </summary>
    public Application()
    {
        _instance = this;
        _space = Space.Empty();
        _wapp = CreateWebApplication();
        _processor = new Processor();
        _network = new Network(_wapp.Services);
        _ticks = (_time = Time.Now).Ticks;
    }

    /// <summary>
    /// The currently running <see cref="Application"/>.
    /// </summary>
    public static Application Current => _instance;

    /// <summary>
    /// The application's <see cref="Spatial.Configuration"/>.
    /// </summary>
    public Configuration Configuration => _wapp.Services.GetRequiredService<Configuration>();

    /// <summary>
    /// The application's <see cref="Simulation.Space"/>.
    /// </summary>
    public Space Space => _space;

    /// <summary>
    /// The application's <see cref="Compute.Processor"/>.
    /// </summary>
    public Processor Processor => _processor;

    /// <summary>
    /// The application's <see cref="Networking.Network"/>.
    /// </summary>
    public Network Network => _network;

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
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/>.</param>
    public static async void Run<T>(CancellationToken cancellationToken = default) where T : Application, new()
    {
        try
        {
            var application = new T();
            var tickRate = (Time) (application.Configuration.TickRate / 60);

            ConfigureTelemetry();

            try
            {
                INFO("Time: {Time}", Time.Now.Milliseconds);
                INFO("Version: {Version}", application.Configuration.Version);
                INFO("Application starting.");

                try
                {
                    application.Start();
                    application._wapp.Start();
                }
                catch (Exception exception)
                {
                    ERROR(exception, "Failed to start the application.");
                    return;
                }

                application.ConfigureSystems();                
                application._network.Open(application.Configuration.Network.Endpoint);
                application._processor.Run();

                if (cancellationToken == default)
                {
                    cancellationToken = CreateCancellationToken();
                }
            }
            catch (Exception exception)
            {
                ERROR(exception, "Failed to run the application.");
                return;
            }

            if (application.Configuration.TickRate > 0)
            {
                INFO("Running at {TickRate} TPS.", application.Configuration.TickRate);

                Ticker.Run(application.Tick, 1000.0D / application.Configuration.TickRate, cancellationToken);
            }
            else
            {
                INFO("Running as fast as possible.");

                Ticker.Run(application.Tick, cancellationToken);
            }

            INFO("Shutting down the application.");

            application.Shutdown();
            await application._wapp.StopAsync(CancellationToken.None);
            application._processor.Shutdown();
            application._network.Close();

            INFO("Application shut down.");

            Log.CloseAndFlush();
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Failed to run the application.\n{exception}");
        }
    }

    /// <summary>
    /// Add configurable options to the <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected void AddOptions<T>(IHostApplicationBuilder builder) where T : class
    {
        builder.Services
            .AddTransient<T>(x => x.GetRequiredService<IOptionsMonitor<T>>().CurrentValue)
            .AddOptions<T>()
            .Bind(builder.Configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    /// <summary>
    /// Configure the <see cref="Application"/>.
    /// </summary>
    /// <param name="builder">The builder configuring the <see cref="Application"/>.</param>
    public virtual void Configure(IHostApplicationBuilder builder) { }

    /// <summary>
    /// Start the <see cref="Application"/>.
    /// </summary>
    public virtual void Start() { }

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
            .WriteTo.File(
                path: Constants.LogFilePath,
                rollingInterval: RollingInterval.Infinite,
                rollOnFileSizeLimit: true);

        try
        {
            config.WriteTo.MongoDBCapped(Current.Configuration.Database.ConnectionString, collectionName: Constants.LogCollectionName);
        }
        catch (OptionsValidationException) { }

#if DEBUG
        config.MinimumLevel.Is(LogEventLevel.Debug);
#endif

        Log.Logger = config.CreateLogger();

        INFO("Telemetry enabled.");
    }

    private void ConfigureSystems()
    {
        AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(type => type.GetCustomAttribute<DependencyAttribute>() is not null)
            .OrderBy(type => type.GetCustomAttribute<DependencyAttribute>()!.Layer)
            .ForEach(Use);

        INFO("Systems online.");
    }

    private WebApplication CreateWebApplication()
    {
        var builder = WebApplication.CreateBuilder();

        Configure(builder);

        builder.Configuration.AddJsonFile(
            path: "appsettings.override.json",
            optional: true,
            reloadOnChange: true);

        AddOptions<Configuration>(builder);

        builder.Services
            .AddSerilog()
            .AddExceptionHandler<FaultIndicator>()
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

    private void Use(Type system)
    {
        _space.Use(_ => (System<Space>) ActivatorUtilities.CreateInstance(_wapp.Services, system));
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

        _space.Update(delta);
        Update(delta);

        _network.Send();

        _time += delta;
        _ticks++;
    }
}