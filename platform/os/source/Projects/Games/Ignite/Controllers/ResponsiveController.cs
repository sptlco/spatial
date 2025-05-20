// Copyright © Spatial. All rights reserved.

using Ignite.Assets.Types;
using Ignite.Components;
using Ignite.Contracts;
using Ignite.Contracts.Actions;
using Ignite.Contracts.Avatars;
using Ignite.Contracts.Characters;
using Ignite.Contracts.Combat;
using Ignite.Contracts.Maps;
using Ignite.Contracts.Miscellaneous;
using Ignite.Contracts.Options;
using Ignite.Contracts.Prison;
using Ignite.Contracts.Sessions;
using Ignite.Contracts.Users;
using Ignite.Models;
using Spatial.Extensions;
using Spatial.Networking;
using System.Linq;
using System.Text;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> capable of providing feedback to the user.
/// </summary>
public class ResponsiveController : AugmentedController
{
    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_USER_CLIENT_WRONGVERSION_CHECK_ACK"/>.
    /// </summary>
    protected void NC_USER_CLIENT_WRONGVERSION_CHECK_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_USER_CLIENT_WRONGVERSION_CHECK_ACK,
            data: new PROTO_NC_USER_CLIENT_WRONGVERSION_CHECK_ACK());
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK"/>.
    /// </summary>
    protected void NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK,
            data: new PROTO_NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK
            {
                XTrapServerKeyLength = 0,
                XTrapServerKey = []
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_USER_XTRAP_ACK"/>.
    /// </summary>
    protected void NC_USER_XTRAP_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_USER_XTRAP_ACK,
            data: new PROTO_NC_USER_XTRAP_ACK {
                bSuccess = true
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_USER_LOGIN_ACK"/>.
    /// </summary>
    protected void NC_USER_LOGIN_ACK()
    {
        var worlds = GetWorlds();

        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_USER_LOGIN_ACK,
            data: new PROTO_NC_USER_LOGIN_ACK {
                numofworld = (byte) worlds.Length,
                worldinfo = worlds
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_USER_LOGINFAIL_ACK"/>.
    /// </summary>
    /// <param name="error"></param>
    protected void NC_USER_LOGINFAIL_ACK(ushort error)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_USER_LOGINFAIL_ACK,
            data: new PROTO_NC_USER_LOGINFAIL_ACK {
                error = error
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_USER_WORLD_STATUS_ACK"/>.
    /// </summary>
    protected void NC_USER_WORLD_STATUS_ACK()
    {
        var worlds = GetWorlds();

        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_USER_WORLD_STATUS_ACK,
            data: new PROTO_NC_USER_WORLD_STATUS_ACK {
                numofworld = (byte) worlds.Length,
                worldinfo = worlds
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_USER_WORLDSELECT_ACK"/>.
    /// </summary>
    protected void NC_USER_WORLDSELECT_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_USER_WORLDSELECT_ACK,
            data: new PROTO_NC_USER_WORLDSELECT_ACK {
                worldstatus = World.Status,
                ip = Server.Endpoint.Address.ToString(),
                port = (ushort) Server.Endpoint.Port,
                validate_new = _session.GenerateKey()
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_USER_LOGINWORLD_ACK"/>.
    /// </summary>
    protected void NC_USER_LOGINWORLD_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_USER_LOGINWORLD_ACK,
            data: new PROTO_NC_USER_LOGINWORLD_ACK {
                worldmanager = _session.Handle,
                numofavatar = (byte) _session.Account.Characters.Count,
                avatar = _session.Account.Characters.ToDenseArray(a => new PROTO_AVATARINFORMATION(a))
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_USER_WILL_WORLD_SELECT_ACK"/>.
    /// </summary>
    protected void NC_USER_WILL_WORLD_SELECT_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_USER_WILL_WORLD_SELECT_ACK,
            data: new PROTO_NC_USER_WILL_WORLD_SELECT_ACK {
                nError = 7768,
                sOTP = _session.Reference().GenerateOTP()
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_MISC_GAMETIME_ACK"/>.
    /// </summary>
    protected void NC_MISC_GAMETIME_ACK()
    {
        var totalSeconds = (long) (World.Time / 1000D);
        var totalMinutes = totalSeconds / 60;

        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_MISC_GAMETIME_ACK,
            data: new PROTO_NC_MISC_GAMETIME_ACK {
                hour = (byte) (totalMinutes / 60 % 24),
                minute = (byte) (totalMinutes % 60),
                second = (byte) (totalSeconds % 60)
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_AVATAR_CREATESUCC_ACK"/>.
    /// </summary>
    /// <param name="character">The <see cref="Character"/> that was created.</param>
    protected void NC_AVATAR_CREATESUCC_ACK(Character character)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_AVATAR_CREATESUCC_ACK,
            data: new PROTO_NC_AVATAR_CREATESUCC_ACK {
                numofavatar = (byte) _session.Account.Characters.Count,
                avatar = new PROTO_AVATARINFORMATION(character)
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_AVATAR_ERASESUCC_ACK"/>.
    /// </summary>
    /// <param name="slot">The deleted character's slot.</param>
    protected void NC_AVATAR_ERASESUCC_ACK(byte slot)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_AVATAR_ERASESUCC_ACK,
            data: new PROTO_NC_AVATAR_ERASESUCC_ACK {
                slot = slot
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_SHORTCUTDATA_ACK"/>.
    /// </summary>
    protected void NC_CHAR_OPTION_GET_SHORTCUTDATA_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_GET_SHORTCUTDATA_ACK,
            data: new PROTO_NC_CHAR_OPTION_GET_SHORTCUTDATA_ACK {
                bSuccess = true,
                Data = new PROTO_NC_CHAR_OPTION_SHORTCUTDATA {
                    Data = _session.Character.Options.Shortcuts
                }
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_SHORTCUTSIZE_ACK"/>.
    /// </summary>
    protected void NC_CHAR_OPTION_GET_SHORTCUTSIZE_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_GET_SHORTCUTSIZE_ACK,
            data: new PROTO_NC_CHAR_OPTION_GET_SHORTCUTSIZE_ACK {
                bSuccess = true,
                Data = new PROTO_NC_CHAR_OPTION_SHORTCUTSIZE {
                    Data = _session.Character.Options.ShortcutSize
                }
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_VIDEO_ACK"/>.
    /// </summary>
    protected void NC_CHAR_OPTION_GET_VIDEO_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_GET_VIDEO_ACK,
            data: new PROTO_NC_CHAR_OPTION_GET_VIDEO_ACK {
                bSuccess = true,
                Data = new PROTO_NC_CHAR_OPTION_VIDEO {
                    Data = _session.Character.Options.Video
                }
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_SOUND_ACK"/>.
    /// </summary>
    protected void NC_CHAR_OPTION_GET_SOUND_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_GET_SOUND_ACK,
            data: new PROTO_NC_CHAR_OPTION_GET_SOUND_ACK {
                bSuccess = true,
                Data = new PROTO_NC_CHAR_OPTION_SOUND {
                    Data = _session.Character.Options.Sound
                }
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_GAME_ACK"/>.
    /// </summary>
    protected void NC_CHAR_OPTION_GET_GAME_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_GET_GAME_ACK,
            data: new PROTO_NC_CHAR_OPTION_GET_GAME_ACK {
                bSuccess = true,
                Data = new PROTO_NC_CHAR_OPTION_GAME {
                    Data = _session.Character.Options.Game
                }
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_WINDOWPOS_ACK"/>.
    /// </summary>
    protected void NC_CHAR_OPTION_GET_WINDOWPOS_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_GET_WINDOWPOS_ACK,
            data: new PROTO_NC_CHAR_OPTION_GET_WINDOWPOS_ACK {
                bSuccess = true,
                Data = new PROTO_NC_CHAR_OPTION_WINDOWPOS {
                    Data = _session.Character.Options.Windows
                }
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_GET_KEYMAPPING_ACK">.
    /// </summary>
    protected void NC_CHAR_OPTION_GET_KEYMAPPING_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_GET_KEYMAPPING_ACK,
            data: new PROTO_NC_CHAR_OPTION_GET_KEYMAPPING_ACK {
                bSuccess = true,
                Data = new PROTO_NC_CHAR_OPTION_KEYMAPPING {
                    Data = _session.Character.Options.Keys
                }
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_ACK"/>.
    /// </summary>
    /// <param name="error">A classifying error code.</param>
    protected void NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_ACK(ushort error)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_ACK,
            data: new PROTO_NC_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_ACK {
                nError = error
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_ACK"/>.
    /// </summary>
    /// <param name="error">A classifying error code.</param>
    protected void NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_ACK(ushort error)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_ACK,
            data: new PROTO_NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_ACK {
                nError = error
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_ACK"/>.
    /// </summary>
    /// <param name="error">A classifying error code.</param>
    protected void NC_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_ACK(ushort error)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_ACK,
            data: new PROTO_NC_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_ACK {
                nError = error
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_ACK"/>.
    /// </summary>
    /// <param name="error">A classifying error code.</param>
    protected void NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_ACK(ushort error)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_ACK,
            data: new PROTO_NC_CHAR_OPTION_IMPROVE_INIT_KEYMAP_ACK {
                nError = error,
                DBKeyMapData = new PROTO_NC_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD {
                    nKeyMapDataCnt = 0,
                    KeyMapData = []
                }
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_ACK"/>.
    /// </summary>
    /// <param name="error">A classifying error code.</param>
    protected void NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_ACK(ushort error)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_ACK,
            data: new PROTO_NC_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_ACK {
                nError = error,
                DBGameOptionData = new PROTO_NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD {
                    nGameOptionDataCnt = 0,
                    GameOptionData = []
                }
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD"/>.
    /// </summary>
    protected void NC_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD,
            data: new PROTO_NC_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD {
                nShortCutDataCnt = 0,
                ShortCutData = []
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD"/>.
    /// </summary>
    protected void NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD,
            data: new PROTO_NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD {
                nGameOptionDataCnt = 0,
                GameOptionData = []
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD"/>.
    /// </summary>
    protected void NC_CHAR_OPTION_IMPROVE_GET_KEYMAPPING_CMD()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD,
            data: new PROTO_NC_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD {
                nKeyMapDataCnt = 0,
                KeyMapData = []
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_PRISON_GET_ACK"/>.
    /// </summary>
    /// <param name="error">A classifying error code.</param>
    protected void NC_PRISON_GET_ACK(ushort error)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_PRISON_GET_ACK,
            data: new PROTO_NC_PRISON_GET_ACK {
                err = error,
                nMinute = 0,
                sReason = "Treason",
                sRemark = "You've commited treason!"
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_USER_CONNECTION_CHANGE_CHAR_ID_ACK"/>.
    /// </summary>
    /// <param name="error">A classifying error code.</param>
    /// <param name="slot">The avatar's slot number.</param>
    /// <param name="name">The avatar's new name.</param>
    protected void NC_USER_CONNECTION_CHANGE_CHAR_ID_ACK(ushort error, byte slot, string name)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_USER_CONNECTION_CHANGE_CHAR_ID_ACK,
            data: new PROTO_NC_USER_CONNECTION_CHANGE_CHAR_ID_ACK {
                nError = error,
                nSlotNo = slot,
                sNewID = name
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_LOGIN_ACK"/>.
    /// </summary>
    protected void NC_CHAR_LOGIN_ACK()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_LOGIN_ACK,
            data: new PROTO_NC_CHAR_LOGIN_ACK {
                zoneip = Server.Endpoint.Address.ToString(),
                zoneport = (ushort) Server.Endpoint.Port
            });
    }

    private static PROTO_NC_USER_LOGIN_ACK.WorldInfo[] GetWorlds()
    {
        return [new PROTO_NC_USER_LOGIN_ACK.WorldInfo {
            worldname = Constants.WorldName,
            worldstatus = World.Status
        }];
    }
}
