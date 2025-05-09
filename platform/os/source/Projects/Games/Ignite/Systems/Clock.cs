// Copyright © Spatial. All rights reserved.

using Ignite.Components;
using Ignite.Contracts;
using Ignite.Contracts.Miscellaneous;
using Ignite.Models;
using Spatial.Simulation;

namespace Ignite.Systems;

/// <summary>
/// A <see cref="System"/> that 
/// </summary>
public class Clock : System
{
    const double MINUTE = 60.0D * 1000.0D;

    private readonly Query _query;
    private double _next;

    /// <summary>
    /// Create a new <see cref="Clock"/>.
    /// </summary>
    public Clock(Map map) : base(map)
    {
        var time = World.Time.Milliseconds;

        _query = new Query().WithAll<Player>();
        _next = time - (time % MINUTE) + MINUTE;
    }

    /// <summary>
    /// Update the <see cref="Map"/>.
    /// </summary>
    /// <param name="space">The <see cref="Map"/> to update.</param>
    /// <param name="delta"><see cref="Time"/> since the last update.</param>
    public override void Update(Map map, Time delta)
    {
        while (World.Time.Milliseconds >= _next)
        {
            _next += MINUTE;

            map.Space.Mutate(_query, (Future f, in Entity e, ref Player p) => {
                World.Command(
                    connection: Session.Find(p.Session).World,
                    command: NETCOMMAND.NC_MISC_SERVER_TIME_NOTIFY_CMD,
                    data: new PROTO_NC_MISC_SERVER_TIME_NOTIFY_CMD());
            });
        }
    }
}