// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Components;
using Ignite.Contracts;
using Ignite.Contracts.Maps;
using Ignite.Models;
using Serilog;
using Spatial.Networking;

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
                NC_MAP_LOGINFAIL_ACK(i);
                return;
            }
        }

        _session.Reference();

        _session.Map = _connection;
        _session.Object = Player.Create(_session, _session.Character, Map.InstanceAt(_session.Character.Map));

        NC_CHAR_CLIENT_BASE_CMD();
        NC_CHAR_CLIENT_SHAPE_CMD();
        NC_CHAR_CLIENT_ITEM_CMD(_session.Character.Inventory);
        NC_CHAR_CLIENT_ITEM_CMD(_session.Character.Equipment);
        NC_MAP_LOGIN_ACK(Param.Stats(_session.Character.Class, _session.Character.Level));
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_MAP_LOGINCOMPLETE_CMD"/>
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_MAP_LOGINCOMPLETE_CMD)]
    public void NC_MAP_LOGINCOMPLETE_CMD(PROTO_NC_MAP_LOGINCOMPLETE_CMD _)
    {
        NC_BRIEFINFO_MOB_CMD();

        if (_session.Character.Power > 0)
        {
            NC_CHAR_ADMIN_LEVEL_INFORM_CMD();
        }

        NC_BAT_HPCHANGE_CMD();

        if (_session.Character.Class >= Class.Crusader)
        {
            NC_BAT_LPCHANGE_CMD();
        }
        else
        {
            NC_BAT_SPCHANGE_CMD();
        }

        NC_MISC_SERVER_TIME_NOTIFY_CMD();

        Log.Information("{Character} logged into {Map} as {Object}.", _session.Character.Name, _session.Object.Map.Name, _session.Object);
    }
}
