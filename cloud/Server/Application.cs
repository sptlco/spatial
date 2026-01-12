// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Data.Accounts;
using Spatial.Cloud.ECS.Systems;
using Spatial.Identity;
using Spatial.Persistence;
using System.Reflection;

namespace Spatial.Cloud;

/// <summary>
/// An autonomous <see cref="Application"/> that lives in the cloud.
/// </summary>
internal class Server : Application
{
    private readonly Dictionary<int, Transducer> _transducers;

    /// <summary>
    /// Create a new <see cref="Server"/>.
    /// </summary>
    public Server()
    {
        _transducers = Assembly
            .GetEntryAssembly()!
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(Transducer)) && type.GetCustomAttribute<ModuleAttribute>() != default)
            .ToDictionary(
                keySelector: type => type.GetCustomAttribute<ModuleAttribute>()!.Id,
                elementSelector: type => (Transducer) Activator.CreateInstance(type)!);
    }

    /// <summary>
    /// Get the current <see cref="Server"/>.
    /// </summary>
    public static new Server Current => (Server) Application.Current;

    /// <summary>
    /// The server's feature <see cref="ECS.Systems.Extractor"/>.
    /// </summary>
    public Extractor Extractor { get; internal set; }

    /// <summary>
    /// The server's <see cref="ECS.Systems.Hypersolver"/>.
    /// </summary>
    public Hypersolver Hypersolver { get; internal set; }

    /// <summary>
    /// The server's transducers, by their neural group.
    /// </summary>
    public Dictionary<int, Transducer> Transducers => _transducers;

    /// <summary>
    /// Configure the server's <see cref="IHostApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder"/> to configure.</param>
    public override void ConfigureBuilder(IHostApplicationBuilder builder)
    {
        AddOptions<ServerConfiguration>(builder);
    }

    /// <summary>
    /// Configure the server's <see cref="WebApplication"/>.
    /// </summary>
    /// <param name="application">The <see cref="WebApplication"/> to configure.</param>
    public override void ConfigureApplication(WebApplication application)
    {
        application.Use(async (context, next) => {
            if (context.Items.TryGetValue(Spatial.Variables.Session, out var sesh) && sesh is Session session)
            {
                context.Items[Variables.Account] = Record<Account>.Read(session.User);
            }

            await next(context);
        });
    }
}