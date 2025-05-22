// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Combat;
using Spatial.Networking;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for combat functions.
/// </summary>
public class CombatController : AugmentedController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_BAT_TARGETTING_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_BAT_TARGETTING_REQ)]
    public void NC_BAT_TARGETTING_REQ(PROTO_NC_BAT_TARGET_REQ data)
    {
        _session.Player.Target(_session.Player.Map.Ref(data.target));
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_BAT_UNTARGET_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_BAT_UNTARGET_REQ)]
    public void NC_BAT_UNTARGET_REQ(PROTO_NC_BAT_UNTARGET_REQ _)
    {
        _session.Player.Untarget();
    }
}