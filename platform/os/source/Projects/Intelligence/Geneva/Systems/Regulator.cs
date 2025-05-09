// Copyright © Spatial. All rights reserved.

using Geneva.Components;
using Serilog;
using Spatial.Simulation;

namespace Geneva.Systems;

/// <summary>
/// A <see cref="System{Space}"/> that returns neurons to rest.
/// </summary>
public class Regulator : System<Space>
{
    private readonly Query _query;

    /// <summary>
    /// Create a new <see cref="Regulator"/>.
    /// </summary>
    public Regulator()
    {
        _query = new Query().WithAll<Neuron>();
    }

    /// <summary>
    /// Update the <see cref="Space"/>.
    /// </summary>
    /// <param name="space">The enclosing <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> passed since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        space.Mutate(_query, (Future f, in Entity e, ref Neuron n) =>
        {
            switch (n.State)
            {
                case State.Resting:
                    n.Charge = Constants.pR + (n.Charge - Constants.pR) * Math.Exp(-Constants.λ * delta.Seconds);
                    
                    if (n.Charge >= Constants.pT)
                    {
                        Log.Debug("Neuron {neuron} depolarizing.", e.Id);

                        n.Charge = Constants.pT;
                        n.State = State.Depolarizing;
                    }

                    break;
                case State.Depolarizing:
                    n.Charge += (Constants.pA - n.Charge) * Constants.rD * delta.Seconds;

                    if (n.Charge >= Constants.pA)
                    {
                        Log.Debug("Neuron {neuron} repolarizing.", e.Id);

                        n.Charge = Constants.pA;
                        n.State = State.Repolarizing;
                    }

                    break;
                case State.Repolarizing:
                    n.Charge += (Constants.pRf - n.Charge) * Constants.rR * delta.Seconds;

                    if (n.Charge <= Constants.pRf)
                    {
                        Log.Debug("Neuron {neuron} hyperpolarizing.", e.Id);

                        n.Charge = Constants.pRf;
                        n.State = State.Hyperpolarizing;
                    }

                    break;
                case State.Hyperpolarizing:
                    n.Charge += (Constants.pR - n.Charge) * Constants.rH * delta.Seconds;

                    if (n.Charge >= Constants.pR)
                    {
                        Log.Debug("Neuron {neuron} resting.", e.Id);
                        
                        n.Charge = Constants.pR;
                        n.State = State.Resting;
                    }
                    
                    break;
            }
        });
    }
}