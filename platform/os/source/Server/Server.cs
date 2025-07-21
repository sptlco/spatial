// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// An autonomous <see cref="Application"/> that lives in the cloud.
/// </summary>
public class Server : Application
{
    /// <summary>
    /// Update the <see cref="Server"/>.
    /// </summary>
    /// <param name="delta">The <see cref="Time"/> passed since the last update.</param>
    public override void Update(Time delta)
    {
        // Every 1 minute, trade on the Ethereum network.
        // Use a momentum strategy to decide whether to buy or sell.

        Interval.Invoke(Trade, Time.FromMinutes(1));
    }

    private void Trade()
    {
        // ...
    }
}