// Copyright © Spatial. All rights reserved.

using Ignite;
using Ignite.Contracts;
using Ignite.Contracts.Miscellaneous;
using Ignite.Models;
using Serilog;
using Spatial.Compute.Jobs;
using Spatial.Networking;
using Spatial.Simulation;
using System;
using System.Diagnostics;
using System.Linq;

Console.WriteLine("\x1b[0m\x1b[1;33m");
Console.WriteLine(@"
    ██╗ ██████╗ ███╗   ██╗ ██╗ ████████╗███████╗
    ██║██╔════╝ ████╗  ██║ ██║ ╚══██╔══╝██╔════╝
    ██║██║  ███╗██╔██╗ ██║ ██║    ██║   █████╗  
    ██║██║   ██║██║╚██╗██║ ██║    ██║   ██╔══╝  
    ██║╚██████╔╝██║ ╚████║ ██║    ██║   ███████╗
    ╚═╝ ╚═════╝ ╚═╝  ╚═══╝ ╚═╝    ╚═╝   ╚══════╝
");
Console.WriteLine("\x1b[0m");

Starter.Invoke(args);

Server.UseUnhandled((_, command) => Log.Error("Unhandled {command}", (NETCOMMAND) command));
Server.UseDisconnect(Disconnect);
Server.Open(Constants.DefaultServerPort);

World.Sync();

var ticks = 0;

Ticker.Run(
    cancellationToken: Spatial.Environment.CancellationToken,
    rate: Constants.Delta,
    function: (delta) => {
        ticks++;

        World.Interval(Constants.ConsoleKey, Time.FromSeconds(1), () => {
            Console.Title = $"TPS: {ticks} | Players: {Session.Count()} | Maps: {World.Maps.Values.Sum(maps => maps.Count)} | Memory: {Process.GetCurrentProcess().PrivateMemorySize64 / 1024.0 / 1024.0:0.00}MB";
            ticks = 0;
        });

        Job.ParallelFor(Session.List(), Session.Prune);

        World.Interval(Constants.MonitorKey, Constants.MonitorInterval, () => {
            World.Multicast(
                command: NETCOMMAND.NC_MISC_HEARTBEAT_REQ,
                data: new PROTO_NC_MISC_HEARTBEAT_REQ(),
                filter: c => c.Get<bool>(Properties.Monitor));
        });

        Server.Receive();
        World.Update(delta);
        Server.Send();
    });

static void Disconnect(Connection connection)
{
    if (connection.Metadata.TryGetValue(Properties.Session, out var handle) && handle is ushort session)
    {
        Session.Find(session).Release();
    }
}