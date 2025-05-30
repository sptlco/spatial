// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Components;
using Ignite.Contracts;
using Ignite.Contracts.Actions;
using Ignite.Models;
using Serilog;
using Spatial.Networking;
using System.Linq;
using System.Text;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for action functions.
/// </summary>
public class ActionController : AugmentedController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_CHAT_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_CHAT_REQ)]
    public void NC_ACT_CHAT_REQ(PROTO_NC_ACT_CHAT_REQ data)
    {
        var message = Encoding.ASCII.GetString(data.content, data.itemLinkDataCount, data.len - data.itemLinkDataCount);

        if (message.StartsWith(Constants.CommandPrefix))
        {
            var args = message.Split();
            var command = args[0].Replace(Constants.CommandPrefix, "");
            var settings = Asset.FirstOrDefault<AdminLvSet>("AdminLvSet.shn", cmd => cmd.ALS_Cmd == command);

            if (settings != null)
            {
                if (_session.Character.Power >= settings.ALS_Lv)
                {
                    ExecuteCommand(command, args);
                }

                return;
            }
        }

        // ...

        Log.Information("{Player}: {Message}", _session.Character.Name, message);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_SHOUT_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_SHOUT_CMD)]
    public void NC_ACT_SHOUT_CMD(PROTO_NC_ACT_SHOUT_CMD data)
    {
        // ...

        Log.Information("{Player}: {Message}", _session.Character.Name, Encoding.ASCII.GetString(data.content, data.itemLinkDataCount, data.len - data.itemLinkDataCount));
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_MOVEWALK_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_MOVEWALK_CMD)]
    public void NC_ACT_MOVEWALK_CMD(PROTO_NC_ACT_MOVEWALK_CMD data)
    {
        // ...
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_MOVERUN_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_MOVERUN_CMD)]
    public void NC_ACT_MOVERUN_CMD(PROTO_NC_ACT_MOVERUN_CMD data)
    {
        // ...
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_STOP_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_STOP_REQ)]
    public void NC_ACT_STOP_REQ(PROTO_NC_ACT_STOP_REQ data)
    {
        // ...
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_NPCCLICK_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_NPCCLICK_CMD)]
    public void NC_ACT_NPCCLICK_CMD(PROTO_NC_ACT_NPCCLICK_CMD data)
    {
    }

    private void ExecuteCommand(string command, string[] args)
    {
        // ...
    }
}