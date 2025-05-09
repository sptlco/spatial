// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Options;
using Spatial.Extensions;
using Spatial.Networking;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for option functions.
/// </summary>
public class OptionController : ResponsiveController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_SHORTCUTDATA_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_GET_SHORTCUTDATA_REQ)]
    public void NC_CHAR_OPTION_GET_SHORTCUTDATA_REQ(PROTO_NC_CHAR_OPTION_GET_SHORTCUTDATA_REQ _)
    {
        NC_CHAR_OPTION_GET_SHORTCUTDATA_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_SHORTCUTSIZE_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_GET_SHORTCUTSIZE_REQ)]
    public void NC_CHAR_OPTION_GET_SHORTCUTSIZE_REQ(PROTO_NC_CHAR_OPTION_GET_SHORTCUTSIZE_REQ _)
    {
        NC_CHAR_OPTION_GET_SHORTCUTSIZE_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_VIDEO_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_GET_VIDEO_REQ)]
    public void NC_CHAR_OPTION_GET_VIDEO_REQ(PROTO_NC_CHAR_OPTION_GET_VIDEO_REQ _)
    {
        NC_CHAR_OPTION_GET_VIDEO_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_SOUND_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_GET_SOUND_REQ)]
    public void NC_CHAR_OPTION_GET_SOUND_REQ(PROTO_NC_CHAR_OPTION_GET_SOUND_REQ _)
    {
        NC_CHAR_OPTION_GET_SOUND_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_GAME_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_GET_GAME_REQ)]
    public void NC_CHAR_OPTION_GET_GAME_REQ(PROTO_NC_CHAR_OPTION_GET_GAME_REQ _)
    {
        NC_CHAR_OPTION_GET_GAME_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_WINDOWPOS_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_GET_WINDOWPOS_REQ)]
    public void NC_CHAR_OPTION_GET_WINDOWPOS_REQ(PROTO_NC_CHAR_OPTION_GET_WINDOWPOS_REQ _)
    {
        NC_CHAR_OPTION_GET_WINDOWPOS_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_KEYMAPPING_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_GET_KEYMAPPING_REQ)]
    public void NC_CHAR_OPTION_GET_KEYMAPPING_REQ(PROTO_NC_CHAR_OPTION_GET_KEYMAPPING_REQ data)
    {
        NC_CHAR_OPTION_GET_KEYMAPPING_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_REQ)]
    public void NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_REQ(PROTO_NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_REQ data)
    {
        NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_ACK(0);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_REQ)]
    public void NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_REQ(PROTO_NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_REQ data)
    {
        NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_ACK(0);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_REQ)]
    public void NC_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_REQ(PROTO_NC_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_REQ data)
    {
        NC_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_ACK(0);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_REQ)]
    public void NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_REQ(PROTO_NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_REQ data)
    {
        NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_ACK(0);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_REQ)]
    public void NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_REQ(PROTO_NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_REQ data)
    {
        NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_ACK(0);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_SET_SHORTCUTSIZE_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_SET_SHORTCUTSIZE_CMD)]
    public void NC_CHAR_OPTION_SET_SHORTCUTSIZE_CMD(PROTO_NC_CHAR_OPTION_SET_SHORTCUTSIZE_CMD data)
    {
        _session.Character.Update(c => c.Options.ShortcutSize = data.Data.Data);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_SET_WINDOWPOS_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_SET_WINDOWPOS_CMD)]
    public void NC_CHAR_OPTION_SET_WINDOWPOS_CMD(PROTO_NC_CHAR_OPTION_SET_WINDOWPOS_CMD data)
    {
        _session.Character.Update(c => c.Options.Windows = data.Data.Data);
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_OPTION_SET_KEYMAPPING_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_OPTION_SET_KEYMAPPING_CMD)]
    public void NC_CHAR_OPTION_SET_KEYMAPPING_CMD(PROTO_NC_CHAR_OPTION_SET_KEYMAPPING_CMD data)
    {
        _session.Character.Update(c => c.Options.Keys = data.Data.Data);
    }
}