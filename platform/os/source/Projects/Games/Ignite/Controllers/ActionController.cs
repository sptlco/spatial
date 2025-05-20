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
                if (_character.Power >= settings.ALS_Lv)
                {
                    ExecuteCommand(command, args);
                }

                return;
            }
        }

        _map.Multicast2D(
            command: NETCOMMAND.NC_ACT_SOMEONECHAT_CMD,
            position: _player.Transform,
            data: new PROTO_NC_ACT_SOMEONECHAT_CMD {
                itemLinkDataCount = data.itemLinkDataCount,
                handle = _player.Tag.Handle,
                len = data.len,
                flag = new PROTO_NC_ACT_SOMEONECHAT_CMD.Flags {
                    chatwin = true
                },
                nChatFontColorID = _character.ChatColor.Font,
                nChatBalloonColorID = _character.ChatColor.Balloon,
                content = data.content
            });

        Log.Information("{Player}: {Message}", _player, message);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_SHOUT_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_SHOUT_CMD)]
    public void NC_ACT_SHOUT_CMD(PROTO_NC_ACT_SHOUT_CMD data)
    {
        _map.Broadcast(
            command: NETCOMMAND.NC_ACT_SOMEONESHOUT_CMD,
            data: new PROTO_NC_ACT_SOMEONESHOUT_CMD {
                itemLinkDataCount = data.itemLinkDataCount,
                speaker = new PROTO_NC_ACT_SOMEONESHOUT_CMD.Speaker {
                    charID = _character.Name
                },
                flag = new PROTO_NC_ACT_SOMEONESHOUT_CMD.Flags(),
                len = data.len,
                content = data.content
            });

        Log.Information("{Player}: {Message}", _player, Encoding.ASCII.GetString(data.content, data.itemLinkDataCount, data.len - data.itemLinkDataCount));
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_MOVEWALK_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_MOVEWALK_CMD)]
    public void NC_ACT_MOVEWALK_CMD(PROTO_NC_ACT_MOVEWALK_CMD data)
    {
        _player.Snap(data.from.x, data.from.y);
        _player.Walk(data.to.x, data.to.y);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_MOVERUN_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_MOVERUN_CMD)]
    public void NC_ACT_MOVERUN_CMD(PROTO_NC_ACT_MOVERUN_CMD data)
    {
        _player.Snap(data.from.x, data.from.y);
        _player.Run(data.to.x, data.to.y);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_STOP_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_STOP_REQ)]
    public void NC_ACT_STOP_REQ(PROTO_NC_ACT_STOP_REQ data)
    {
        _player.Stop(data.loc.x, data.loc.y);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_ACT_NPCCLICK_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_ACT_NPCCLICK_CMD)]
    public void NC_ACT_NPCCLICK_CMD(PROTO_NC_ACT_NPCCLICK_CMD data)
    {
        _player.Interact(_map.ObjectAt<NPCRef>(data.npchandle));
    }

    private void ExecuteCommand(string command, string[] args)
    {
        switch (command)
        {
            case "adminlevel":
                _player.Notify($"Admin level is {_character.Power}.");
                break;
            case "coord":
                _player.Notify($"Location[{_player.Tag.Handle}] : {_map.Name}/{_player.Transform.X}/{_player.Transform.Y}/{_player.Transform.R}");
                break;
            case "linkto":
                var instance = Map.InstanceAtOrDefault(args[1]);

                if (instance is null)
                {
                    _player.Notify($"The map does not exist.");
                    return;
                }

                _player.Teleport(
                    map: instance.Serial,
                    id: instance.Id,
                    transform: (float.TryParse(args.ElementAtOrDefault(2), out var x) && float.TryParse(args.ElementAtOrDefault(3), out var y)) ? new Transform(x, y) : null);

                break;
        }
    }
}