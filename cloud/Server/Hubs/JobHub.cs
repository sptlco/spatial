// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.SignalR;
using Spatial.Networking;

namespace Spatial.Cloud.Hubs;

[Path("/hub/jobs")]
public class JobHub : Hub
{
    public async Task UpdateStatusAsync()
    {
        // ...
    }
}