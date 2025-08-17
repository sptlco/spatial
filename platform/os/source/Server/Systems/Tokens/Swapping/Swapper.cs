// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.Extensions.Options;
using Spatial.Contracts;
using Spatial.Simulation;

namespace Spatial.Systems.Tokens.Swapping;

/// <summary>
/// An automated token swapping system.
/// </summary>
[Dependency(1)]
public class Swapper : System
{
    private readonly IOptionsMonitor<CloudConfiguration> _config;

    /// <summary>
    /// Create a new <see cref="Swapper"/>.
    /// </summary>
    /// <param name="config">Configurable options for the <see cref="Swapper"/>.</param>
    public Swapper(IOptionsMonitor<CloudConfiguration> config)
    {
        _config = config;
    }

    /// <summary>
    /// Update the <see cref="Space"/>.
    /// </summary>
    /// <param name="space">The <see cref="Space"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        Interval.Invoke(
            interval: Time.FromMinutes(1),
            function: () => {

                // Execute a trade every minute on Ethereum.
                // Use a momentum strategy to determine whether to buy or sell.

                // ...

            });
    }
}