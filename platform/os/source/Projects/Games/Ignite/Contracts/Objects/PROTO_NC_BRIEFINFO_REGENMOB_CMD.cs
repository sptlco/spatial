// Copyright © Spatial. All rights reserved.

using Ignite;
using Ignite.Components;
using Ignite.Contracts;
using Ignite.Models;
using Ignite.Models.Objects;
using Spatial.Networking;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_BRIEFINFO_REGENMOB_CMD"/>.
/// </summary>
public class PROTO_NC_BRIEFINFO_REGENMOB_CMD : ProtocolBuffer
{
    /// <summary>
    /// The mob's handle.
    /// </summary>
    public ushort handle;

    /// <summary>
    /// The mob's current mode.
    /// </summary>
    public byte mode;

    /// <summary>
    /// The mob's identification number.
    /// </summary>
    public ushort mobid;

    /// <summary>
    /// The mob's coordinates.
    /// </summary>
    public SHINE_COORD_TYPE coord;

    /// <summary>
    /// The mob's flag state.
    /// </summary>
    public byte flagstate;

    /// <summary>
    /// The mob's abnormal states.
    /// </summary>
    public ABNORMAL_STATE_BIT abstatebit;

    /// <summary>
    /// The map the gate links to.
    /// </summary>
    public string gate2where;

    /// <summary>
    /// The mob's animation.
    /// </summary>
    public string sAnimation;

    /// <summary>
    /// The mob's animation level.
    /// </summary>
    public byte nAnimationLevel;

    /// <summary>
    /// The mob's kingdom quest team.
    /// </summary>
    public byte nKQTeamType;

    /// <summary>
    /// Whether or not the mob is regenerating.
    /// </summary>
    public bool bRegenAni;

    /// <summary>
    /// Create a new <see cref="PROTO_NC_BRIEFINFO_REGENMOB_CMD"/>.
    /// </summary>
    public PROTO_NC_BRIEFINFO_REGENMOB_CMD() { }

    /// <summary>
    /// Create a new <see cref="PROTO_NC_BRIEFINFO_REGENMOB_CMD"/>.
    /// </summary>
    /// <param name="reference">An <see cref="ObjectRef"/> reference.</param>
    public PROTO_NC_BRIEFINFO_REGENMOB_CMD(ObjectRef reference)
    {
        handle = reference.Tag.Handle;
        mobid = (reference as MobRef)?.Value.Id ?? (reference as NPCRef)!.Value.Id;
        coord = new SHINE_COORD_TYPE {
            dir = reference.Transform.D,
            xy = new SHINE_XY_TYPE {
                x = (uint) reference.Transform.X,
                y = (uint) reference.Transform.Y
            }
        };
        sAnimation = "";
        nKQTeamType = (byte) KQ_TEAM_TYPE.KQTT_MAX;
        bRegenAni = false;

        if (reference.Has<Gate>())
        {
            var gate = reference.Get<Gate>();

            flagstate = 1;
            gate2where = Map.InstanceAt(gate.Map, gate.Id).Data.Field.MapIDClient;
        }
        else
        {
            abstatebit = new ABNORMAL_STATE_BIT {
                statebit = new byte[Constants.AbnormalStateBits]
            };
        }
    }

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        handle = ReadUInt16();
        mode = ReadByte();
        mobid = ReadUInt16();
        coord = Read<SHINE_COORD_TYPE>();
        flagstate = ReadByte();

        if (flagstate == 1)
        {
            gate2where = ReadString(12);

            ReadBytes(Constants.AbnormalStateBits - 12);
        }
        else
        {
            abstatebit = Read<ABNORMAL_STATE_BIT>();
        }

        sAnimation = ReadString(32);
        nAnimationLevel = ReadByte();
        nKQTeamType = ReadByte();
        bRegenAni = ReadBoolean();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(handle);
        Write(mode);
        Write(mobid);
        Write(coord);
        Write(flagstate);

        if (flagstate == 1)
        {
            Write(gate2where, 12);
            
            Fill(Constants.AbnormalStateBits - 12, 0);
            
        }
        else
        {
            Write(abstatebit);
        }

        Write(sAnimation, 32);
        Write(nAnimationLevel);
        Write(nKQTeamType);
        Write(bRegenAni);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        coord.Dispose();
        abstatebit?.Dispose();

        base.Dispose();
    }
}