// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Components;
using Spatial.Simulation;

namespace Spatial.Cloud.Systems;

/// <summary>
/// ...
/// </summary>
[Dependency]
public class Hypersolver : System
{
    private readonly Query _neurons;
    private readonly Query _synapses;

    /// <summary>
    /// Create a new <see cref="Hypersolver"/>.
    /// </summary>
    public Hypersolver()
    {
        _neurons = new Query().WithAll<Neuron>();
        _synapses = new Query().WithAll<Synapse>();
    }

    /// <summary>
    /// Update the <see cref="Hypersolver"/>.
    /// </summary>
    /// <param name="space">The current <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        // ...
        
        space.Mutate(_synapses, (Future future, in Entity entity, ref Synapse synapse) => {

            // ...

        });

        space.Mutate(_neurons, (Future future, in Entity entity, ref Neuron neuron) => {

            // ...

        });
    }
}