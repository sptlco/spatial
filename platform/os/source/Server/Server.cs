// Copyright © Spatial Corporation. All rights reserved.

using Serilog;

namespace Spatial;

public class Server : Application
{
    public override void Update(Time delta)
    {
        Log.Information("Hello, world!");
    }
}