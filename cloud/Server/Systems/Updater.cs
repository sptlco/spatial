// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Extensions;
using Spatial.Simulation;

namespace Spatial.Cloud.Systems;

/// <summary>
/// Propagates updates to transducers.
/// </summary>
[Dependency(2)]
public class Updater : System
{
    /// <summary>
    /// Update the server's transducers.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        Server.Current.Transducers.ForEach((_, transducer) => transducer.Update(delta));
    }
}