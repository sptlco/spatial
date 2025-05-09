// Copyright © Spatial. All rights reserved.

using Geneva.Components;
using Geneva.Contracts;
using Geneva.Contracts.Motors;
using Spatial.Networking;
using Spatial.Simulation;

namespace Geneva.Systems;

/// <summary>
/// A <see cref="System{Space}"/> that broadcasts the state of the <see cref="Brain"/>.
/// </summary>
public class Caster : System<Space>
{
    private readonly Query _query;

    /// <summary>
    /// Create a new <see cref="Caster"/>.
    /// </summary>
    public Caster()
    {
        _query = new Query().WithAll<Neuron, Receptor>();
    }

    /// <summary>
    /// Update the <see cref="Space"/>.
    /// </summary>
    /// <param name="space">The enclosing <see cref="Space"/>.</param>
    /// <param name="delta"><see cref="Time"/> passed since the last update.</param>
    public override void Update(Space space, Time delta)
    {
        var state = new double[space.Count<Receptor>(r => r.Type == ReceptorType.Motor)];
        
        space.Mutate(_query, (Future f, in Entity e, ref Neuron n, ref Receptor r) => 
        {
            if (r.Type == ReceptorType.Motor)
            {
                state[r.Channel] = n.Charge;
            }
        });

        Server.Broadcast(
            command: (ushort) NETCOMMAND.NC_MOTOR_OUTPUT_CMD, 
            data: new PROTO_NC_MOTOR_OUTPUT_CMD { 
                Count = state.Length, 
                Outputs = state });
    }
}
