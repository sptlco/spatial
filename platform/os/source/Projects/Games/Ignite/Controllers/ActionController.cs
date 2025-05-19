// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Components;
using Ignite.Contracts;
using Ignite.Contracts.Actions;
using Ignite.Models;
using Ignite.Models.Objects;
using Serilog;
using Spatial.Networking;
using System.Linq;
using System.Text;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for action functions.
/// </summary>
public class ActionController : ResponsiveController
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

        _session.Ref.Map.Multicast2D(
            command: NETCOMMAND.NC_ACT_SOMEONECHAT_CMD,
            position: _session.Ref.Transform,
            data: new PROTO_NC_ACT_SOMEONECHAT_CMD {
                itemLinkDataCount = data.itemLinkDataCount,
                handle = _session.Ref.Tag.Handle,
                len = data.len,
                flag = new PROTO_NC_ACT_SOMEONECHAT_CMD.Flags {
                    chatwin = true
                },
                nChatFontColorID = _session.Character.ChatColor.Font,
                nChatBalloonColorID = _session.Character.ChatColor.Balloon,
                content = data.content
            });

        Log.Information("{Player}: {Message}", _session.Character.Name, message);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_SHOUT_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_SHOUT_CMD)]
    public void NC_ACT_SHOUT_CMD(PROTO_NC_ACT_SHOUT_CMD data)
    {
        _session.Ref.Map.Broadcast(
            command: NETCOMMAND.NC_ACT_SOMEONESHOUT_CMD,
            data: new PROTO_NC_ACT_SOMEONESHOUT_CMD {
                itemLinkDataCount = data.itemLinkDataCount,
                speaker = new PROTO_NC_ACT_SOMEONESHOUT_CMD.Speaker {
                    charID = _session.Character.Name
                },
                flag = new PROTO_NC_ACT_SOMEONESHOUT_CMD.Flags(),
                len = data.len,
                content = data.content
            });

        Log.Information("{Player}: {Message}", _session.Character.Name, Encoding.ASCII.GetString(data.content, data.itemLinkDataCount, data.len - data.itemLinkDataCount));
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_MOVEWALK_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_MOVEWALK_CMD)]
    public void NC_ACT_MOVEWALK_CMD(PROTO_NC_ACT_MOVEWALK_CMD data)
    {
        _session.Ref.Snap(data.from.x, data.from.y);
        _session.Ref.Walk(data.to.x, data.to.y);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_MOVERUN_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_MOVERUN_CMD)]
    public void NC_ACT_MOVERUN_CMD(PROTO_NC_ACT_MOVERUN_CMD data)
    {
        _session.Ref.Snap(data.from.x, data.from.y);
        _session.Ref.Run(data.to.x, data.to.y);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_STOP_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_STOP_REQ)]
    public void NC_ACT_STOP_REQ(PROTO_NC_ACT_STOP_REQ data)
    {
        _session.Ref.Stop(data.loc.x, data.loc.y);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_NPCCLICK_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_NPCCLICK_CMD)]
    public void NC_ACT_NPCCLICK_CMD(PROTO_NC_ACT_NPCCLICK_CMD data)
    {
        _session.Ref.Interact(_session.Ref.Map.ObjectAt<NPCRef>(data.npchandle));
    }

    private void ExecuteCommand(string command, string[] args)
    {
        switch (command)
        {
            case "adminlevel":
                NC_ACT_NOTICE_CMD($"Admin level is {_session.Character.Power}.");
                break;
            case "coord":
                NC_ACT_NOTICE_CMD($"Location[{_session.Ref.Tag.Handle}] : {_session.Ref.Map.Name}/{_session.Ref.Transform.X}/{_session.Ref.Transform.Y}/{_session.Ref.Transform.R}");
                break;
            case "linkto":
                var instance = Map.InstanceAtOrDefault(args[1]);

                if (instance is null)
                {
                    NC_ACT_NOTICE_CMD($"The map does not exist.");
                    return;
                }

                _session.Ref.Teleport(
                    map: instance.Serial,
                    id: instance.Id,
                    transform: (float.TryParse(args.ElementAtOrDefault(2), out var x) && float.TryParse(args.ElementAtOrDefault(3), out var y)) ? new Transform(x, y) : null);

                break;
        }
    }
}