// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Miscellaneous;
using Ignite.Models;
using Spatial.Networking;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for miscellaneous functions.
/// </summary>
public class ServerController : ResponsiveController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_MISC_GAMETIME_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_MISC_GAMETIME_REQ)]
    public void NC_MISC_GAMETIME_REQ(PROTO_NC_MISC_GAMETIME_REQ _)
    {
        NC_MISC_GAMETIME_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_MISC_HEARTBEAT_ACK"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_MISC_HEARTBEAT_ACK)]
    public void NC_MISC_HEARTBEAT_ACK(PROTO_NC_MISC_HEARTBEAT_ACK _)
    {
        _connection.Set(Properties.Alive, World.Time.Milliseconds);
    }
}