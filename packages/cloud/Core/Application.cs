// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Spatial.Compute;
using Spatial.Extensions;
using Spatial.Identity;
using Spatial.Identity.Authorization;
using Spatial.Networking;
using Spatial.Persistence;
using Spatial.Simulation;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Spatial;

/// <summary>
/// A program bootstrapped with platform features.
/// </summary>
public class Application
{
    private static Application _instance;

    private readonly CancellationTokenSource _shutdown;
    private readonly WebApplication _wapp;
    private Time _time;
    private double _ticks;
    private double? _budget;
    private (string Name, double Elapsed)[] _timings;

    private readonly Ticker _ticker;
    private readonly Computer _computer;
    private readonly Network _network;
    private readonly Space _space;

    /// <summary>
    /// Create a new <see cref="Application"/>.
    /// </summary>
    public Application()
    {
        _instance = this;
        _shutdown = new();
        _space = Space.Empty();
        _wapp = CreateWebApplication();
        _ticker = new Ticker();
        _computer = new Computer();
        _network = new Network();
        _ticks = (_time = Time.Now).Ticks;
        _timings = [];

        Initialize();
    }

    /// <summary>
    /// The currently running <see cref="Application"/>.
    /// </summary>
    public static Application Current => _instance;

    /// <summary>
    /// The application's registered services.
    /// </summary>
    public IServiceProvider Services => _wapp.Services;

    /// <summary>
    /// The application's <see cref="Spatial.Configuration"/>.
    /// </summary>
    public Configuration Configuration => Services.GetRequiredService<Configuration>();

    /// <summary>
    /// The application's <see cref="Compute.Computer"/>.
    /// </summary>
    public Computer Computer => _computer;

    /// <summary>
    /// The application's <see cref="Networking.Network"/>.
    /// </summary>
    public Network Network => _network;

    /// <summary>
    /// The application's <see cref="Simulation.Space"/>.
    /// </summary>
    public Space Space => _space;

    /// <summary>
    /// The application's local time.
    /// </summary>
    public Time Time => _time;

    /// <summary>
    /// The application's tick count.
    /// </summary>
    public double Ticks => _ticks;

    /// <summary>
    /// Run an <see cref="Application"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Application"/> to run.</typeparam>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/>.</param>
    public static async Task RunAsync<T>(CancellationToken cancellationToken = default) where T : Application, new()
    {
        try
        {
            var application = new T();
            var tps = (Time) (application.Configuration.TickRate / 60);

            try
            {
                INFO("Time: {Time} ms.", Time.Now.Milliseconds);
                INFO("Environment: {Environment}.", application._wapp.Environment.EnvironmentName);
                INFO("Starting {Application} {Version}.", application.Configuration.Name, application.Configuration.Version);

                application.Start();
                application._wapp.Start();
                application.Listen();

                using var source = CancellationTokenSource.CreateLinkedTokenSource(
                    token1: cancellationToken.CanBeCanceled ? cancellationToken : CreateCancellationToken(), 
                    token2: application._shutdown.Token);

                var token = source.Token;

                if (application.Configuration.TickRate > 0)
                {
                    INFO("Application running at {TickRate} tps.", application.Configuration.TickRate);

                    application._ticker.Run(
                        function: application.TryTick, 
                        delta: (double) (application._budget = 1000.0D / application.Configuration.TickRate), 
                        cancellationToken: token);
                }
                else
                {
                    INFO("Application running as fast as possible.");

                    application._ticker.Run(application.TryTick, token);
                }

                INFO("Shutting down the application.");

                application._shutdown.Cancel();
                await application._wapp.StopAsync(token);
                application.Shutdown();

                application._computer.Dispose();
                application._network.Dispose();
                application._space.Dispose();

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
    /// Configure the <see cref="IHostApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder"/> to configure.</param>
    public virtual void ConfigureBuilder(IHostApplicationBuilder builder) { }

    /// <summary>
    /// Configure the <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="application">The <see cref="WebApplication"/> to configure.</param>
    public virtual void ConfigureApplication(WebApplication application) { }

    /// <summary>
    /// Start the <see cref="Application"/>.
    /// </summary>
    public virtual void Start() { }

    /// <summary>
    /// Shutdown the <see cref="Application"/>.
    /// </summary>
    public virtual void Shutdown() { }

    /// <summary>
    /// Tick the <see cref="Application"/>.
    /// </summary>
    /// <param name="delta">Elapsed time since the last tick.</param>
    public virtual void Tick(Time delta) { }

    private void Initialize()
    {
        AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(type => type.GetCustomAttribute<RunAttribute>() is not null)
            .OrderBy(type => type.GetCustomAttribute<RunAttribute>()!.Layer)
            .ForEach(system => _space.Use(_ => (Simulation.System) ActivatorUtilities.CreateInstance(_wapp.Services, system)));

        _space.Initialize();
        _timings = new (string, double)[_space.Systems.Count];
    }

    private void Listen()
    {
        foreach (var endpoint in ParseEndpoints(Configuration))
        {
            var uri = new Uri(endpoint
                .Replace("*", IPAddress.Any.ToString())
                .Replace("localhost", IPAddress.Loopback.ToString()));

            if (uri.Scheme.ToLowerInvariant().Equals(Constants.UriSchemes.Socket))
            {
                _network.Listen(new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port).ToString());

                INFO("TCP supported, endpoint: {Endpoint}.", endpoint);
            }
        }
    }

    private WebApplication CreateWebApplication()
    {
        var path = Path.Combine(AppContext.BaseDirectory, Constants.StaticFilePath);
        var builder = WebApplication.CreateSlimBuilder(); 

        Directory.CreateDirectory(path);

        ConfigureBuilder(builder);

        builder.Configuration.AddJsonFile(
            path: Constants.OverridePath,
            optional: true,
            reloadOnChange: true);

        AddOptions<Configuration>(builder);

        var configuration = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<IOptionsMonitor<Configuration>>().CurrentValue;
        
        var telemetry = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
            .WriteTo.Console()
            .WriteTo.File(
                path: Path.Combine(AppContext.BaseDirectory, Constants.LogFilePath),
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true);

        try
        {
            telemetry.WriteTo.MongoDBCapped(configuration.Database.ConnectionString, collectionName: Constants.LogCollectionName);
        }
        catch (OptionsValidationException) { }

#if DEBUG
        telemetry.MinimumLevel.Is(LogEventLevel.Debug);
#endif

        Log.Logger = telemetry.CreateLogger();

        BsonSerializer.TryRegisterSerializer(new DecimalSerializer(BsonType.Decimal128));

        builder.WebHost.ConfigureKestrel(options => {

            foreach (var endpoint in ParseEndpoints(configuration))
            {
                var uri = new Uri(endpoint
                    .Replace("*", IPAddress.Any.ToString())
                    .Replace("localhost", IPAddress.Loopback.ToString()));

                switch (uri.Scheme.ToLowerInvariant())
                {
                    case Constants.UriSchemes.Http:

                        options.Listen(IPAddress.Parse(uri.Host), uri.Port);
                    
                        INFO("HTTP supported, endpoint: {Endpoint}.", endpoint);
                    
                        break;
                    case Constants.UriSchemes.Https:
                        
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
                                options.Listen(IPAddress.Parse(uri.Host), uri.Port, listener => listener.UseHttps(certificate));

                                INFO("HTTPS supported, endpoint: {Endpoint}.", endpoint);
                            }
                        }
                        catch (Exception exception)
                        {
                            WARN(exception, "Failed to locate a valid SSL certificate for HTTPS endpoint {Endpoint}.", endpoint);   
                        }
                        
                        break;
                }
            }
        });

        builder.Services
            .AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters.ValidIssuer = configuration.JWT.Issuer;
                options.TokenValidationParameters.ValidAudience = configuration.JWT.Audience;
                options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JWT.Secret));
                options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var sid = context.Principal?.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;

                        if (string.IsNullOrEmpty(sid))
                        {
                            context.Fail("The access token is missing a valid session identifier.");
                            return Task.CompletedTask;
                        }

                        if (Resource<Session>.FirstOrDefault(sesh => sesh.Id == sid && sesh.Expires > Time.Now) is not Session session)
                        {
                            context.Fail("The user's session does not exist.");
                            return Task.CompletedTask;
                        }

                        context.HttpContext.Items[Variables.Session] = session;

                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services
            .AddAuthorizationBuilder()
            .AddPolicy(Constants.Policies.RBAC, policy => policy.AddRequirements(new PermissionRequirement()));

        builder.Services
            .AddCors()
            .AddSerilog()
            .AddExceptionHandler<FaultIndicator>()
            .AddProblemDetails()
            .AddControllers()
            .AddJsonOptions(options => {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            });

        builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

        var application = builder.Build();

        application
            .UseRouting()
            .UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod())
            .UsePathBase(configuration.BasePath)
            .UseExceptionHandler()
            .UseStatusCodePages(ReportStatusCode)
            .UseFileServer(new FileServerOptions {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = PathString.Empty,
                EnableDefaultFiles = true
            })
            .UseSerilogRequestLogging()
            .UseAuthentication()
            .UseMiddleware<Enricher>()
            .UseAuthorization()
            .UseHsts();

        if (ParseEndpoints(configuration).Any(e => e.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
        {
            application.UseHttpsRedirection();
        }

        ConfigureApplication(application);

        application.MapControllers();

        application.Use(async (context, next) => {
            if (context.Request.Method == HttpMethods.Options)
            {
                context.Response.StatusCode = StatusCodes.Status204NoContent;
                return;
            }

            await next();
        });

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

        using var _ = LogContext.PushProperty(Constants.Properties.TraceId, traceId);

        switch ((HttpStatusCode) status.HttpContext.Response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                await context.Response.WriteAsJsonAsync(new NotFound().ToFault().ToResponse(traceId));
                break;
            case HttpStatusCode.Unauthorized:
                await context.Response.WriteAsJsonAsync(new Unauthorized().ToFault().ToResponse(traceId));
                break;
            case HttpStatusCode.Forbidden:
                await context.Response.WriteAsJsonAsync(new Forbidden().ToFault().ToResponse(traceId));
                break;
        }
    }

    private void TryTick(Time delta)
    {
        try
        {
            var receive = Profiler.Measure("Receive", () => _network.Receive());
            var update = Profiler.Measure("Update", () => _space.Update(delta, _timings));
            var send = Profiler.Measure("Send", () => _network.Send());

            var elapsed = receive.Elapsed + update.Elapsed + send.Elapsed;

            if (_budget is double budget && elapsed > budget)
            {
                var breakdown = new StringBuilder();

                breakdown.AppendLine($"  - Receive ({receive.Elapsed:F2}ms)");
                breakdown.AppendLine($"  - Update ({update.Elapsed:F2}ms)");

                foreach (var (name, ms) in _timings)
                {
                    breakdown.AppendLine($"    - {name} ({ms:F2}ms)");
                }

                breakdown.Append($"  - Send ({send.Elapsed:F2}ms)");

                WARN($"Tick exceeded {{Budget}}ms budget by {{Difference}}ms (took {{Elapsed}}ms)\n{{Breakdown}}", budget, elapsed - budget, elapsed, breakdown.ToString());
            }

            Tick(delta);
        }
        catch (Exception exception)
        {
            ERROR(exception, "Failed to tick.");
        }

        _time += delta;
        _ticks++;
    }

    private string[] ParseEndpoints(Configuration configuration)
    {
        return [.. Regex.Replace(configuration.Endpoints, @"\s+", "").Split(",").Where(e => !string.IsNullOrEmpty(e))];
    }
}
