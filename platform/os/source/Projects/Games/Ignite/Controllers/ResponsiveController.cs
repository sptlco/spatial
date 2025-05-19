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

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_CLIENT_BASE_CMD"/>.
    /// </summary>
    protected void NC_CHAR_CLIENT_BASE_CMD()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_CLIENT_BASE_CMD,
            data: new PROTO_NC_CHAR_BASE_CMD {
                chrregnum = _session.Character.Id,
                charid = _session.Character.Name,
                slotno = _session.Character.Slot,
                Level = _session.Character.Level,
                Experience = _session.Character.XP,
                CurHPStone = _session.Character.HPStones,
                CurSPStone = _session.Character.SPStones,
                CurHP = _session.Character.HP,
                CurSP = _session.Character.SP,
                CurLP = _session.Character.LP,
                fame = _session.Character.Fame,
                Cen = _session.Character.Money,
                logininfo = new PROTO_NC_CHAR_BASE_CMD.LoginLocation {
                    currentmap = Field.Find(_session.Character.Map).MapIDClient,
                    currentcoord = new SHINE_COORD_TYPE {
                        dir = _session.Ref.Transform.D,
                        xy = new SHINE_XY_TYPE {
                            x = (uint) _session.Ref.Transform.X,
                            y = (uint) _session.Ref.Transform.Y
                        }
                    }
                },
                statdistribute = new CHARSTATDISTSTR {
                    Strength = _session.Character.Archetype.Strength,
                    Constitute = _session.Character.Archetype.Endurance,
                    Dexterity = _session.Character.Archetype.Dexterity,
                    Intelligence = _session.Character.Archetype.Intelligence,
                    MentalPower = _session.Character.Archetype.Spirit,
                    RedistributePoint = _session.Character.Archetype.Points
                },
                pkyellowtime = (byte) _session.Character.KillCooldown.TotalMinutes,
                pkcount = _session.Character.Kills,
                prisonmin = (ushort) _session.Character.Sentence.TotalMinutes,
                adminlevel = _session.Character.Power,
                flags = new PROTO_NC_CHAR_BASE_CMD.Flags()
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_CLIENT_SHAPE_CMD"/>.
    /// </summary>
    protected void NC_CHAR_CLIENT_SHAPE_CMD()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_CLIENT_SHAPE_CMD,
            data: new PROTO_AVATAR_SHAPE_INFO {
                race = _session.Character.Race,
                chrclass = _session.Character.Class,
                gender = _session.Character.Gender,
                hairtype = _session.Character.Appearance.Hair.Style,
                haircolor = _session.Character.Appearance.Hair.Color,
                faceshape = _session.Character.Appearance.Face
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_CLIENT_ITEM_CMD"/>.
    /// </summary>
    /// <param name="type">An <see cref="InventoryType"/>.</param>
    protected void NC_CHAR_CLIENT_ITEM_CMD(Inventory inventory)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_CLIENT_ITEM_CMD,
            data: new PROTO_NC_CHAR_CLIENT_ITEM_CMD {
                numofitem = (byte) inventory.Count,
                box = (byte) inventory.Type,
                invenclear = true,
                ItemArray = inventory.ToArray(item => new PROTO_ITEMPACKET_INFORM(item))
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_MAP_LOGIN_ACK"/>.
    /// </summary>
    /// <param name="parameters">The character's base parameters.</param>
    protected void NC_MAP_LOGIN_ACK(Param parameters)
    {
        var @base = new Abilities(parameters);

        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_MAP_LOGIN_ACK,
            data: new PROTO_NC_CHAR_MAPLOGIN_ACK {
                charhandle = _session.Ref.Tag.Handle,
                param = new CHAR_PARAMETER_DATA {
                    PrevExp = StatTable.XP(_session.Ref.Vitals.Level),
                    NextExp = StatTable.XP((byte) (_session.Ref.Vitals.Level + 1)),
                    Strength = new SHINE_CHAR_STATVAR { Base = (uint) parameters.Strength, Change = (uint) _session.Ref.Attributes.Strength },
                    Constitute = new SHINE_CHAR_STATVAR { Base = (uint) parameters.Constitution, Change = (uint) _session.Ref.Attributes.Endurance },
                    Dexterity = new SHINE_CHAR_STATVAR { Base = (uint) parameters.Dexterity, Change = (uint) _session.Ref.Attributes.Dexterity },
                    Intelligence = new SHINE_CHAR_STATVAR { Base = (uint) parameters.Intelligence, Change = (uint) _session.Ref.Attributes.Intelligence },
                    Wizdom = new SHINE_CHAR_STATVAR { Base = (uint) parameters.Wizdom, Change = (uint) _session.Ref.Attributes.Wisdom },
                    MentalPower = new SHINE_CHAR_STATVAR { Base = (uint) parameters.MentalPower, Change = (uint) _session.Ref.Attributes.Spirit },
                    WClow = new SHINE_CHAR_STATVAR { Base = (uint) @base.Damage.Minimum, Change = (uint) _session.Ref.Abilities.Damage.Minimum },
                    WChigh = new SHINE_CHAR_STATVAR { Base = (uint) @base.Damage.Maximum, Change = (uint) _session.Ref.Abilities.Damage.Maximum },
                    AC = new SHINE_CHAR_STATVAR { Base = (uint) @base.Defense, Change = (uint) _session.Ref.Abilities.Defense },
                    TH = new SHINE_CHAR_STATVAR { Base = (uint) @base.Aim, Change = (uint) _session.Ref.Abilities.Aim },
                    TB = new SHINE_CHAR_STATVAR { Base = (uint) @base.Evasion, Change = (uint) _session.Ref.Abilities.Evasion },
                    MAlow = new SHINE_CHAR_STATVAR { Base = (uint) @base.MagicDamage.Minimum, Change = (uint) _session.Ref.Abilities.MagicDamage.Minimum },
                    MAhigh = new SHINE_CHAR_STATVAR { Base = (uint) @base.MagicDamage.Maximum, Change = (uint) _session.Ref.Abilities.MagicDamage.Maximum },
                    MR = new SHINE_CHAR_STATVAR { Base = (uint) @base.MagicDefense, Change = (uint) _session.Ref.Abilities.MagicDefense },
                    MaxHp = (uint) _session.Ref.Vitals.Health.Maximum,
                    MaxSp = (uint) _session.Ref.Vitals.Spirit.Maximum,
                    MaxLp = (uint) _session.Ref.Vitals.Light.Maximum,
                    MaxHPStone = (uint) _session.Ref.Stones.Health.Maximum,
                    MaxSPStone = (uint) _session.Ref.Stones.Spirit.Maximum,
                    PainRes = new SHINE_CHAR_STATVAR(),
                    RestraintRes = new SHINE_CHAR_STATVAR(),
                    CurseRes = new SHINE_CHAR_STATVAR(),
                    ShockRes = new SHINE_CHAR_STATVAR()
                },
                logincoord = new SHINE_XY_TYPE {
                    x = (uint) _session.Ref.Transform.X,
                    y = (uint) _session.Ref.Transform.Y
                }
            });
    }

    protected void NC_MAP_LOGINFAIL_ACK(SHN_DATA_FILE_INDEX file)
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_MAP_LOGINFAIL_ACK,
            data: new PROTO_NC_MAP_LOGINFAIL_ACK {
                err = 0x147,
                nWrongDataFileIndex = (byte) file
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_BRIEFINFO_MOB_CMD"/>.
    /// </summary>
    protected void NC_BRIEFINFO_MOB_CMD()
    {
        var mobs = _session.Ref.Map.Query(ObjectType.Mob, ObjectType.NPC).ToList();

        for (var i = 0; i < mobs.Count; i += 0x1C)
        {
            var chunk = mobs.Skip(i).Take(0x1C).ToArray(mob => new PROTO_NC_BRIEFINFO_REGENMOB_CMD(_session.Ref.Map.ObjectAt(mob)));

            World.Command(
                connection: _connection,
                command: NETCOMMAND.NC_BRIEFINFO_MOB_CMD,
                data: new PROTO_NC_BRIEFINFO_MOB_CMD {
                    mobnum = (byte) chunk.Length,
                    mobs = chunk
                });
        }
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_CHAR_ADMIN_LEVEL_INFORM_CMD"/>.
    /// </summary>
    protected void NC_CHAR_ADMIN_LEVEL_INFORM_CMD()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_ADMIN_LEVEL_INFORM_CMD,
            data: new PROTO_NC_CHAR_ADMIN_LEVEL_INFORM_CMD {
                nAdminLevel = _session.Character.Power
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_BAT_HPCHANGE_CMD"/>.
    /// </summary>
    protected void NC_BAT_HPCHANGE_CMD()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_BAT_HPCHANGE_CMD,
            data: new PROTO_NC_BAT_HPCHANGE_CMD {
                hp = (uint) _session.Ref.Vitals.Health.Current,
                hpchangeorder = 0
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_BAT_SPCHANGE_CMD"/>.
    /// </summary>
    protected void NC_BAT_SPCHANGE_CMD()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_BAT_SPCHANGE_CMD,
            data: new PROTO_NC_BAT_SPCHANGE_CMD {
                sp = (uint) _session.Ref.Vitals.Spirit.Current
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_BAT_LPCHANGE_CMD"/>.
    /// </summary>
    protected void NC_BAT_LPCHANGE_CMD()
    {
        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_BAT_LPCHANGE_CMD,
            data: new PROTO_NC_BAT_LPCHANGE_CMD {
                nLP = (uint) _session.Ref.Vitals.Light.Current
            });
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_MISC_SERVER_TIME_NOTIFY_CMD"/>.
    /// </summary>
    protected void NC_MISC_SERVER_TIME_NOTIFY_CMD()
    {
        World.Command(
            connection: _session.World,
            command: NETCOMMAND.NC_MISC_SERVER_TIME_NOTIFY_CMD,
            data: new PROTO_NC_MISC_SERVER_TIME_NOTIFY_CMD());
    }

    /// <summary>
    /// Issue <see cref="NETCOMMAND.NC_ACT_NOTICE_CMD"/>.
    /// </summary>
    /// <param name="notice">A notice.</param>
    protected void NC_ACT_NOTICE_CMD(string notice)
    {
        var bytes = Encoding.ASCII.GetBytes(notice);

        World.Command(
            connection: _session.Map,
            command: NETCOMMAND.NC_ACT_NOTICE_CMD,
            data: new PROTO_NC_ACT_NOTICE_CND {
                len = (byte) bytes.Length,
                content = bytes
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
