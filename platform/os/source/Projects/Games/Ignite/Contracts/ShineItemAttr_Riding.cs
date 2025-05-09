// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_Riding : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's hunger.
    /// </summary>
    public ushort hungrypoint;

    /// <summary>
    /// The item's expiration date.
    /// </summary>
    public ShineDateTime deletetime;

    /// <summary>
    /// The item's flags.
    /// </summary>
    public Flags bitflag;

    /// <summary>
    /// The item's binding flags.
    /// </summary>
    public SHINE_PUT_ON_BELONGED_ITEM IsPutOnBelonged;

    /// <summary>
    /// The item's health.
    /// </summary>
    public uint nHP;

    /// <summary>
    /// The item's grade.
    /// </summary>
    public byte nGrade;

    /// <summary>
    /// The item's failed upgrade count.
    /// </summary>
    public ushort nRareFailCount;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        hungrypoint = ReadUInt16();
        deletetime = Read<ShineDateTime>();
        bitflag = Read<Flags>();
        IsPutOnBelonged = (SHINE_PUT_ON_BELONGED_ITEM) ReadInt32();
        nHP = ReadUInt32();
        nGrade = ReadByte();
        nRareFailCount = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(hungrypoint);
        Write(deletetime);
        Write(bitflag);
        Write((int) IsPutOnBelonged);
        Write(nHP);
        Write(nGrade);
        Write(nRareFailCount);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        deletetime.Dispose();
        bitflag.Dispose();

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing mount data.
    /// </summary>
    public class Flags : ProtocolBuffer
    {
        /// <summary>
        /// The number of times the mount has been ridden.
        /// </summary>
        public ushort ridenum;

        /// <summary>
        /// Whether or not the mount is being ridden.
        /// </summary>
        public bool duringriding;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            var flags = ReadUInt16();

            ridenum = (ushort) (flags & 0x7FFF);
            duringriding = ((flags >> 15) & 0x1) != 0;
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write((((ushort) (duringriding ? 1 : 0) & 0x1) << 15) | (ridenum & 0x7FFF));
        }
    }
}
