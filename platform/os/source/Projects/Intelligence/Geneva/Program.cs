// Copyright © Spatial. All rights reserved.

using Geneva.Models;
using Spatial.Networking;
using Spatial.Simulation;

Console.WriteLine("\x1b[0m\x1b[1;33m");
Console.WriteLine(@"
     ██████╗ ███████╗███╗   ██╗███████╗██╗   ██╗ █████╗
    ██╔════╝ ██╔════╝████╗  ██║██╔════╝██║   ██║██╔══██╗
    ██║  ███╗█████╗  ██╔██╗ ██║█████╗  ██║   ██║███████║
    ██║   ██║██╔══╝  ██║╚██╗██║██╔══╝  ╚██╗ ██╔╝██╔══██║
    ╚██████╔╝███████╗██║ ╚████║███████╗ ╚████╔╝ ██║  ██║
     ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚══════╝  ╚═══╝  ╚═╝  ╚═╝
");
Console.WriteLine("\x1b[0m");

// Configure the brain.
// Configure it with however many sensor neurons, motor neurons, 
// and hidden layers we like.

Schematic.Create(
    sensors: 5, 
    motors: 5,
    layers: [30, 20, 10]);

// Open the server to the public.
// This will allow users to send commands to the network to 
// interact with the brain.

Server.Open();

// A ticker is a mechanism used to run code at a fixed interval, 
// while automatically maintaining a consistent tick rate under the 
// hood using accumulated time.

Ticker.Run(
    // Process the brain at a fixed interval of 30 ticks/sec.
    // Using a fixed timestep ensures frame-rate independence, as well as
    // consistent updates no matter the supporting hardware.

    rate: 1 / 60D * 1000D,

    // Use the global cancellation token to stop ticking 
    // once the application exits, and gracefully destroy the brain.

    cancellationToken: Spatial.Environment.CancellationToken,

    // Create a callback function that will be invoked automatically 
    // by the system at the interval defined above. 

    function: (delta) => {
        // Process incoming network traffic.
        // Providing a cancellation token limits the number of events processed, 
        // and prevent the receive operation from hogging the main loop.

        Server.Receive();

        // Update the brain.
        // This call with pass the delta variable to all systems that 
        // support it, namely BeforeUpdate, Update, and AfterUpdate.

        Schematic.Update(delta);

        // Deliver outgoing messages to connections.
        // This includes any messages that have been sent during this run, 
        // as well as any messages that weren't delivered last run.

        Server.Send();
    });