// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Menu;
using Spatial.Networking;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for menu functions.
/// </summary>
public class MenuController : AugmentedController
{
    [NETHANDLER(NETCOMMAND.NC_MENU_SERVERMENU_ACK)]
    public void NC_MENU_SERVERMENU_ACK(PROTO_NC_MENU_SERVERMENU_ACK data)
    {
        _session.Callbacks.ElementAt(data.reply)();
        _session.Callbacks.Clear();
    }
}