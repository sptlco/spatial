// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing riding data.
/// </summary>
public class CHARBRIEFINFO_RIDE : ProtocolBuffer
{
    /// <summary>
    /// The character's equipment.
    /// </summary>
    public PROTO_EQUIPMENT equip;

    /// <summary>
    /// The character's ride data.
    /// </summary>
    public RideInfo rideinfo;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        equip = Read<PROTO_EQUIPMENT>();
        rideinfo = Read<RideInfo>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(equip);
        Write(rideinfo);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        equip.Dispose();
        rideinfo.Dispose();

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing ride data.
    /// </summary>
    public class RideInfo : ProtocolBuffer
    {
        /// <summary>
        /// The character's horse.
        /// </summary>
        public ushort horse;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            horse = ReadUInt16();
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write(horse);
        }
    }
}
