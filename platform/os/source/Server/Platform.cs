// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

public class Platform : Application
{
    public override void Start(params string[] args)
    {
        INFO("Welcome to Spatial!");
    }
    
    public override void Update(Time delta)
    {
        INFO("Hello, world!");
    }

    public override void Shutdown()
    {
        INFO("Have a good day!");
    }
}