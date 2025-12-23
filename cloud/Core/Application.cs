// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
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
using System.Text.RegularExpressions;

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
    private double _ticks;

    private readonly Computer _computer;
    private readonly Network _network;

    /// <summary>
    /// Create a new <see cref="Application"/>.
    /// </summary>
    public Application()
    {
        _instance = this;
        _space = Space.Empty();
        _wapp = CreateWebApplication();
        _computer = new Computer();
        _network = new Network(_wapp.Services);
        _ticks = (_time = Time.Now).Ticks;

        Initialize();
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
    /// The application's <see cref="Compute.Computer"/>.
    /// </summary>
    public Computer Computer => _computer;

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
    public double Ticks => _ticks;

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

            try
            {
                INFO("Time: {Time} ms.", Time.Now.Milliseconds);
                INFO("Environment: {Environment}.", application._wapp.Environment.EnvironmentName);
                INFO("Starting {Application} {Version}.", application.Name, application.Version);

                application.Listen();

                application.Start();
                application._wapp.Start();
                application._computer.Run();

                if (cancellationToken == default)
                {
                    cancellationToken = CreateCancellationToken();
                }

                if (application.Configuration.TickRate > 0)
                {
                    INFO("Application running at {TickRate} ticks/s.", application.Configuration.TickRate);

                    Ticker.Run(application.TryTick, 1000.0D / application.Configuration.TickRate, cancellationToken);
                }
                else
                {
                    INFO("Application running as fast as possible.");

                    Ticker.Run(application.TryTick, cancellationToken);
                }

                INFO("Shutting down the application.");

                application.Shutdown();
                await application._wapp.StopAsync(CancellationToken.None);
                application._space.Dispose();
                application._computer.Shutdown();
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
    public static Client Connect(string endpoint) => Connect(IPEndPoint.Parse(endpoint));

    /// <summary>
    /// Connect to an <see cref="Application"/> via STCP.
    /// </summary>
    /// <param name="port">The application's port.</param>
    public static Client Connect(int port) => Connect(new IPEndPoint(IPAddress.Loopback, port));

    /// <summary>
    /// Connect to an <see cref="Application"/> via STCP.
    /// </summary>
    /// <param name="endpoint">The application's endpoint.</param>
    /// <returns>A <see cref="Client"/> connected to the <see cref="Application"/>.</returns>
    public static Client Connect(IPEndPoint endpoint)
    {
        return new Client().Connect(endpoint);
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
    /// Update the <see cref="Application"/>.
    /// </summary>
    /// <param name="delta"></param>
    public virtual void Tick(Time delta) { }

    /// <summary>
    /// Shutdown the <see cref="Application"/>.
    /// </summary>
    public virtual void Shutdown() { }

    private void Initialize()
    {
        AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(type => type.GetCustomAttribute<RunAttribute>() is not null)
            .OrderBy(type => type.GetCustomAttribute<RunAttribute>()!.Layer)
            .ForEach(Use);

        _space.Initialize();
    }

    private void Listen()
    {
        foreach (var endpoint in Regex.Replace(Configuration.Endpoints, @"\s+", "").Split(","))
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
        
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
            .WriteTo.Console()
            .WriteTo.File(
                path: Path.Combine(AppContext.BaseDirectory, Constants.LogFilePath),
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true);

        try
        {
            logger.WriteTo.MongoDBCapped(configuration.Database.ConnectionString, collectionName: Constants.LogCollectionName);
        }
        catch (OptionsValidationException) { }

#if DEBUG
        logger.MinimumLevel.Is(LogEventLevel.Debug);
#endif

        Log.Logger = logger.CreateLogger();

        builder.WebHost.ConfigureKestrel(options => {

            foreach (var endpoint in Regex.Replace(configuration.Endpoints, @"\s+", "").Split(","))
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
            .AddAuthentication(options => 
            {
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

                        if (Record<Session>.FirstOrDefault(sesh => sesh.Id == sid && sesh.Expires > Time.Now) is not Session session)
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
            .AddControllers();

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
            .UseHttpsRedirection()
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

    private void Use(Type system)
    {
        _space.Use(_ => (System) ActivatorUtilities.CreateInstance(_wapp.Services, system));
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
        _network.Receive();
        _space.Update(delta);

        try
        {
            Tick(delta);
        }
        catch (Exception exception)
        {
            ERROR(exception, "Failed to tick.");
        }

        _network.Send();

        _time += delta;
        _ticks++;
    }
}
