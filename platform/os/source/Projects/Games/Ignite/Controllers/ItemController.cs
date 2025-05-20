// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Items;
using Spatial.Networking;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for item functions.
/// </summary>
public class ItemController : AugmentedController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ITEM_EQUIP_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ITEM_EQUIP_REQ)]
    public void NC_ITEM_EQUIP_REQ(PROTO_NC_ITEM_EQUIP_REQ data)
    {
        _player.Equip(_character.Inventory.ItemAt(data.slot));
    }
}