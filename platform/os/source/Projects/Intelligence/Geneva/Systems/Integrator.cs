// Copyright © Spatial. All rights reserved.

using Geneva.Components;
using Spatial.Simulation;

namespace Geneva.Systems;

/// <summary>
/// A <see cref="System{Space}"/> that adds graded potentials.
/// </summary>
public class Integrator : System<Space>
{
    private readonly Query _query;

    /// <summary>
    /// Create a new <see cref="Integrator"/>.
    /// </summary>
    public Integrator()
    {
        _query = new Query().WithAll<Synapse>();
    }

    /// <summary>
    /// Update the <see cref="Space"/>.
    /// </summary>
    /// <param name="space">The enclosing <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> passed since the last run.</param>
    public override void Update(Space space, Time delta)
    {
        space.Mutate(_query, (Future f, in Entity e, ref Synapse s) => 
        {
            var pre = space.Get<Neuron>(s.Source);
            var post = space.Get<Neuron>(s.Destination);
            var amplitude = s.Strength;

            if (post.State == State.Resting)
            {
                // A graded potential (pG) is the difference between a neuron's 
                // current potential and its resting potential (pR).

                var pG = pre.Charge - Constants.pR;

                f.Modify(s.Destination, (ref Neuron n) => n.Charge += pG * amplitude);
            }
        });
    }
}