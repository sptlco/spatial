// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Prison;
using Spatial.Networking;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for prison functions.
/// </summary>
public class PrisonController : ResponsiveController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_PRISON_GET_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_PRISON_GET_REQ)]
    public void NC_PRISON_GET_REQ(PROTO_NC_PRISON_GET_REQ _)
    {
        NC_PRISON_GET_ACK(0);
    }
}
