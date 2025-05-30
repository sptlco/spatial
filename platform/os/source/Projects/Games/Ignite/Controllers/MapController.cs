// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Contracts;
using Ignite.Contracts.Characters;
using Ignite.Contracts.Combat;
using Ignite.Contracts.Maps;
using Ignite.Contracts.Miscellaneous;
using Ignite.Contracts.Objects;
using Ignite.Models;
using Serilog;
using Spatial.Extensions;
using Spatial.Networking;
using Spatial.Simulation;
using System.Linq;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for map functions.
/// </summary>
public class MapController : ResponsiveController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_MAP_LOGIN_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_MAP_LOGIN_REQ)]
    public void NC_MAP_LOGIN_REQ(PROTO_NC_MAP_LOGIN_REQ data)
    {
        Monitor().Authenticate(data.chardata.wldmanhandle);

        for (var i = SHN_DATA_FILE_INDEX.SHN_Abstate; i < SHN_DATA_FILE_INDEX.SHN_MaxCnt; i++)
        {
            var asset = Asset.Get<Asset>($"{i.ToString().Replace("SHN_", "")}.shn");

            if (asset != null && string.Compare(asset.Hash, data.checksum[(int) i], System.StringComparison.OrdinalIgnoreCase) != 0)
            {
                World.Command(
                    connection: _connection,
                    command: NETCOMMAND.NC_MAP_LOGINFAIL_ACK,
                    data: new PROTO_NC_MAP_LOGINFAIL_ACK {
                        err = 0x147,
                        nWrongDataFileIndex = (byte) i
                    });

                return;
            }
        }

        _session.Reference();

        _session.Map = _connection;
        _session.Player = Entity.Null;

        // ...
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_MAP_LOGINCOMPLETE_CMD"/>
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_MAP_LOGINCOMPLETE_CMD)]
    public void NC_MAP_LOGINCOMPLETE_CMD(PROTO_NC_MAP_LOGINCOMPLETE_CMD _)
    {
        // ...
    }
}
