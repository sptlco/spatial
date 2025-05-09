// Copyright © Spatial. All rights reserved.

using Geneva.Components;
using Geneva.Models;
using Serilog;
using Spatial.Simulation;
using System.Collections.Concurrent;

namespace Geneva.Systems;

/// <summary>
/// A <see cref="System{Space}"/> that trains the <see cref="Schematic"/>.
/// </summary>
public class Tuner : System<Space>
{
    private readonly Query _query1;
    private readonly Query _query2;
    private readonly ConcurrentDictionary<uint, Time> _spikes;
    private readonly ConcurrentDictionary<uint, State> _states;

    public Tuner()
    {
        _query1 = new Query().WithAll<Neuron>();
        _query2 = new Query().WithAll<Synapse>();
        _spikes = [];
        _states = [];
    }

    public override void Update(Space space, Time delta)
    {
        space.Mutate(_query1, (Future f, in Entity e, ref Neuron n) =>
        {
            if (_states.TryGetValue(e.Id, out var state) && state == State.Resting && n.State == State.Depolarizing)
            {
                _spikes[e.Id] = space.Time;

                Log.Debug("Neuron {neuron} fired at {time}.", e.Id, space.Time.Milliseconds);
            }
            
            _states[e.Id] = n.State;
        });

        space.Mutate(_query2, (Future f, in Entity e, ref Synapse s) =>
        {
            var pre = s.Source;
            var post = s.Destination;

            // General decay.
            // The synaptic connection is becoming weaker as the brain
            // forgets information over time.
                    
            s.Strength -= Constants.Ø * delta.Seconds * s.Strength;

            if (_spikes.TryGetValue(pre, out var tPre) && _spikes.TryGetValue(post, out var tPost))
            {
                var Δt = tPost - tPre;

                if (Δt > 0 && Δt <= Constants.τ)
                {
                    // Long-term potentiation.
                    // The synaptic connection is becoming stronger due to 
                    // causal activation (pre-synaptic to post-synaptic).

                    s.Strength += Constants.A * Math.Exp(-Δt / Constants.τ) * (1 - s.Strength);

                    Log.Debug("Synapse {synapse} grew to {strength:P2}.", e.Id, s.Strength);
                }
            }

            s.Strength = Math.Clamp(s.Strength, 0, 1);
        });

        foreach (var kvp in _spikes.Where(kvp => kvp.Value < (space.Time - Constants.τ)).ToList())
        {
            _spikes.TryRemove(kvp.Key, out _);
        }
    }
}