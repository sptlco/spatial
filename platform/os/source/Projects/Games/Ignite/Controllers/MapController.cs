// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Components;
using Ignite.Contracts;
using Ignite.Contracts.Characters;
using Ignite.Contracts.Combat;
using Ignite.Contracts.Maps;
using Ignite.Contracts.Miscellaneous;
using Ignite.Models;
using Serilog;
using Spatial.Extensions;
using Spatial.Networking;
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
        _session.Ref = Player.Ref(_session, Map.InstanceAt(_session.Character.Map));

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

        var test = new PROTO_NC_CHAR_CLIENT_ITEM_CMD(_session.Character.Inventory) {
            invenclear = true
        };

        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_CLIENT_ITEM_CMD,
            data: test);

        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_CHAR_CLIENT_ITEM_CMD,
            data: new PROTO_NC_CHAR_CLIENT_ITEM_CMD(_session.Character.Equipment) {
                invenclear = true
            });

        var parameters = Param.Stats(_session.Character.Class, _session.Character.Level);
        var abilities = new Abilities(parameters);

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
                    WClow = new SHINE_CHAR_STATVAR { Base = (uint) abilities.Damage.Minimum, Change = (uint) _session.Ref.Abilities.Damage.Minimum },
                    WChigh = new SHINE_CHAR_STATVAR { Base = (uint) abilities.Damage.Maximum, Change = (uint) _session.Ref.Abilities.Damage.Maximum },
                    AC = new SHINE_CHAR_STATVAR { Base = (uint) abilities.Defense, Change = (uint) _session.Ref.Abilities.Defense },
                    TH = new SHINE_CHAR_STATVAR { Base = (uint) abilities.Aim, Change = (uint) _session.Ref.Abilities.Aim },
                    TB = new SHINE_CHAR_STATVAR { Base = (uint) abilities.Evasion, Change = (uint) _session.Ref.Abilities.Evasion },
                    MAlow = new SHINE_CHAR_STATVAR { Base = (uint) abilities.MagicDamage.Minimum, Change = (uint) _session.Ref.Abilities.MagicDamage.Minimum },
                    MAhigh = new SHINE_CHAR_STATVAR { Base = (uint) abilities.MagicDamage.Maximum, Change = (uint) _session.Ref.Abilities.MagicDamage.Maximum },
                    MR = new SHINE_CHAR_STATVAR { Base = (uint) abilities.MagicDefense, Change = (uint) _session.Ref.Abilities.MagicDefense },
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

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_MAP_LOGINCOMPLETE_CMD"/>
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_MAP_LOGINCOMPLETE_CMD)]
    public void NC_MAP_LOGINCOMPLETE_CMD(PROTO_NC_MAP_LOGINCOMPLETE_CMD _)
    {
        var mobs = _session.Ref.Map.Query(ObjectType.Mob, ObjectType.NPC).ToList();

        for (var i = 0; i < mobs.Count; i += 0x1C)
        {
            var part = mobs.Skip(i).Take(0x1C).ToArray(mob => new PROTO_NC_BRIEFINFO_REGENMOB_CMD(_session.Ref.Map.ObjectAt(mob)));

            World.Command(
                connection: _connection,
                command: NETCOMMAND.NC_BRIEFINFO_MOB_CMD,
                data: new PROTO_NC_BRIEFINFO_MOB_CMD {
                    mobnum = (byte) part.Length,
                    mobs = part
                });
        }

        if (_session.Character.Power > 0)
        {
            World.Command(
                connection: _connection,
                command: NETCOMMAND.NC_CHAR_ADMIN_LEVEL_INFORM_CMD,
                data: new PROTO_NC_CHAR_ADMIN_LEVEL_INFORM_CMD {
                    nAdminLevel = _session.Character.Power
                });
        }

        World.Command(
            connection: _connection,
            command: NETCOMMAND.NC_BAT_HPCHANGE_CMD,
            data: new PROTO_NC_BAT_HPCHANGE_CMD {
                hp = (uint) _session.Ref.Vitals.Health.Current,
                hpchangeorder = _session.Ref.Vitals.Version
            });

        if (_session.Character.Class >= Class.Crusader)
        {
            World.Command(
                connection: _connection,
                command: NETCOMMAND.NC_BAT_LPCHANGE_CMD,
                data: new PROTO_NC_BAT_LPCHANGE_CMD {
                    nLP = (uint) _session.Ref.Vitals.Light.Current
                });
        }
        else
        {
            World.Command(
                connection: _connection,
                command: NETCOMMAND.NC_BAT_SPCHANGE_CMD,
                data: new PROTO_NC_BAT_SPCHANGE_CMD {
                    sp = (uint) _session.Ref.Vitals.Spirit.Current
                });
        }

        World.Command(
            connection: _session.World,
            command: NETCOMMAND.NC_MISC_SERVER_TIME_NOTIFY_CMD,
            data: new PROTO_NC_MISC_SERVER_TIME_NOTIFY_CMD());

        Log.Information("{Player} logged into {Map}.", _session.Ref, _session.Ref.Map.Name);
    }
}
