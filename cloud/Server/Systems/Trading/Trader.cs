// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Contracts;
using Spatial.Simulation;

namespace Spatial.Systems.Trading;

/// <summary>
/// An automated trading system.
/// </summary>
[Dependency]
public class Trader : System
{
    private readonly IOptionsMonitor<CloudConfiguration> _config;

    /// <summary>
    /// Create a new <see cref="Trader"/>.
    /// </summary>
    /// <param name="config">Configurable options for the <see cref="Trader"/>.</param>
    public Trader(IOptionsMonitor<CloudConfiguration> config)
    {
        _config = config;
    }

    // DC: Legion
        


    /// <summary>
    /// Update the <see cref="Space"/>.
    /// </summary>
    /// <param name="space">The <see cref="Space"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        foreach (var token in _config.CurrentValue.Systems.Trading.Watch)
        {
            Interval.Invoke(
                interval: Time.FromMinutes(1),
                function: () => {

                    // ...

                });
        }
    }
}