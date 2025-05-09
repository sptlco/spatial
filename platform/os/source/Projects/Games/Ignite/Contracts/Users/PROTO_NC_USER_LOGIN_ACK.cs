// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Users;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_USER_LOGIN_ACK"/>.
/// </summary>
public class PROTO_NC_USER_LOGIN_ACK : ProtocolBuffer
{
    /// <summary>
    /// The current world count.
    /// </summary>
    public byte numofworld;

    /// <summary>
    /// A list of worlds.
    /// </summary>
    public WorldInfo[] worldinfo;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        numofworld = ReadByte();
        worldinfo = Read<WorldInfo>(numofworld);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(numofworld);
        Write(worldinfo);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        foreach (var world in worldinfo)
        {
            world.Dispose();
        }

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing world information.
    /// </summary>
    public sealed class WorldInfo : ProtocolBuffer
    {
        /// <summary>
        /// The world's identification number.
        /// </summary>
        public byte worldno;

        /// <summary>
        /// The world's name.
        /// </summary>
        public string worldname;

        /// <summary>
        /// The world's status.
        /// </summary>
        public byte worldstatus;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            worldno = ReadByte();
            worldname = ReadString(16);
            worldstatus = ReadByte();
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(worldno);
            Write(worldname, 16);
            Write(worldstatus);
        }
    }
}
