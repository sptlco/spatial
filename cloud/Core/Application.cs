// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Spatial.Compute;
using Spatial.Extensions;
using Spatial.Networking;
using Spatial.Simulation;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

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

        ConfigureSystems();
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
    /// The application's registered services.
    /// </summary>
    public IServiceProvider Services => _wapp.Services;

    /// <summary>
    /// The name of the <see cref="Application"/>.
    /// </summary>
    public string Name => Configuration.Name;

    /// <summary>
    /// The application's version.
    /// </summary>
    public string Version => Configuration.Version;

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
                INFO("Time: {Time} ms.", Time.Now.Milliseconds);
                INFO("Starting {Application} {Version}.", application.Name, application.Version);

                application.ConfigureConnectivity();

                application.Start();
                application._wapp.Start();
                application._processor.Run();

                if (cancellationToken == default)
                {
                    cancellationToken = CreateCancellationToken();
                }

                if (application.Configuration.TickRate > 0)
                {
                    INFO("Application running at {TickRate} ticks/s.", application.Configuration.TickRate);

                    Ticker.Run(application.InvokeTick, 1000.0D / application.Configuration.TickRate, cancellationToken);
                }
                else
                {
                    INFO("Application running as fast as possible.");

                    Ticker.Run(application.InvokeTick, cancellationToken);
                }

                INFO("Shutting down the application.");

                application.Shutdown();
                await application._wapp.StopAsync(CancellationToken.None);
                application._space.Dispose();
                application._processor.Shutdown();
                application._network.Close();

                INFO("Application shut down.");
            }
            catch (Exception exception)
            {
                ERROR(exception, "Failed to run the application.");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Failed to run the application.\n{exception}");
        }
    }

    /// <summary>
    /// Connect to an <see cref="Application"/> via STCP.
    /// </summary>
    /// <param name="endpoint">The application's endpoint.</param>
    public static StcpClient Connect(string endpoint) => Connect(IPEndPoint.Parse(endpoint));

    /// <summary>
    /// Connect to an <see cref="Application"/> via STCP.
    /// </summary>
    /// <param name="port">The application's port.</param>
    public static StcpClient Connect(int port) => Connect(new IPEndPoint(IPAddress.Loopback, port));

    /// <summary>
    /// Connect to an <see cref="Application"/> via STCP.
    /// </summary>
    /// <param name="endpoint">The application's endpoint.</param>
    /// <returns>A <see cref="StcpClient"/> connected to the <see cref="Application"/>.</returns>
    public static StcpClient Connect(IPEndPoint endpoint)
    {
        return new StcpClient().Connect(endpoint);
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
    public virtual void Tick(Time delta) { }

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
    }

    private void ConfigureSystems()
    {
        AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(type => type.GetCustomAttribute<DependencyAttribute>() is not null)
            .OrderBy(type => type.GetCustomAttribute<DependencyAttribute>()!.Layer)
            .ForEach(Use);

        _space.Initialize();
    }

    private void ConfigureConnectivity()
    {
        foreach (var url in Configuration.Endpoints)
        {
            switch (new Uri(url.Replace("*", "localhost")).Scheme.ToLowerInvariant())
            {
                case Constants.UriSchemes.Http:
                    _wapp.Urls.Add(url);
                    INFO("HTTP supported, url: {Url}.", url);
                    break;
                case Constants.UriSchemes.Https:
                    _wapp.Urls.Add(url);
                    INFO("HTTPS supported, url: {Url}.", url);
                    break;
                case Constants.UriSchemes.Spatial:
                    _network.Listen(url.Replace($"{Constants.UriSchemes.Spatial}://", "").Replace("*", IPAddress.Any.ToString()));
                    INFO("STCP supported, url: {Url}.", url);
                    break;
            }
        }
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

        builder.WebHost.ConfigureKestrel(options => {
            options.ListenAnyIP(80);

            try
            {
                using var store = new X509Store(StoreLocation.LocalMachine);

                store.Open(OpenFlags.ReadOnly);

                var certificate = store.Certificates
                    .OfType<X509Certificate2>()
                    .Where(c => c.HasPrivateKey && c.NotAfter > DateTime.Now)
                    .OrderByDescending(c => c.NotBefore)
                    .FirstOrDefault();

                if (certificate is not null)
                {
                    options.ListenAnyIP(443, listener => listener.UseHttps(certificate));
                }
            }
            catch (Exception exception)
            {
                WARN(exception, "Failed to locate a valid SSL certificate, HTTPS unsupported.");   
            }
        });

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
            .UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot")),
                RequestPath = ""
            })
            .UseSerilogRequestLogging()
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

        using var _ = LogContext.PushProperty(Constants.Properties.TraceId, traceId);

        switch ((HttpStatusCode) status.HttpContext.Response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                await context.Response.WriteAsJsonAsync(new NotFound().ToFault().ToResponse(traceId));
                break;
        }
    }

    private void InvokeTick(Time delta)
    {
        _network.Receive();
        _space.Update(delta);

        Tick(delta);

        _network.Send();

        _time += delta;
        _ticks++;
    }
}
