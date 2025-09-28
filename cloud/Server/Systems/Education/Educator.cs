// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Simulation;

namespace Spatial.Cloud.Systems.Education;

/// <summary>
/// Teaches inhabitants of the <see cref="Models.Realm"/>.
/// </summary>
[Dependency]
internal class Educator : System
{
    /// <summary>
    /// Update the <see cref="Educator"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> passed since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        // ...
    }
}