// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_Furniture : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's flags.
    /// </summary>
    public Flags flag;

    /// <summary>
    /// The furniture's identification number.
    /// </summary>
    public ushort furnicherID;

    /// <summary>
    /// The furniture's expiration date.
    /// </summary>
    public ShineDateTime deletetime;

    /// <summary>
    /// The piece's X-coordinate.
    /// </summary>
    public float LocX;

    /// <summary>
    /// The piece's Y-coordinate.
    /// </summary>
    public float LocY;

    /// <summary>
    /// The piece's Z-coordinate.
    /// </summary>
    public float LocZ;

    /// <summary>
    /// The piece's direction.
    /// </summary>
    public float Direct;

    /// <summary>
    /// The piece's expiration date.
    /// </summary>
    public ShineDateTime dEndureEndTime;

    /// <summary>
    /// The piece's endurance grade.
    /// </summary>
    public byte nEndureGrade;

    /// <summary>
    /// The piece's value.
    /// </summary>
    public ulong nRewardMoney;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        flag = Read<Flags>();
        furnicherID = ReadUInt16();
        deletetime = Read<ShineDateTime>();
        LocX = ReadSingle();
        LocY = ReadSingle();
        LocZ = ReadSingle();
        Direct = ReadSingle();
        dEndureEndTime = Read<ShineDateTime>();
        nEndureGrade = ReadByte();
        nRewardMoney = ReadUInt64();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(flag);
        Write(furnicherID);
        Write(deletetime);
        Write(LocX);
        Write(LocY);
        Write(LocZ);
        Write(Direct);
        Write(dEndureEndTime);
        Write(nEndureGrade);
        Write(nRewardMoney);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        flag.Dispose();
        deletetime.Dispose();
        dEndureEndTime.Dispose();

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> that contains furniture flag data.
    /// </summary>
    public class Flags : ProtocolBuffer
    {
        /// <summary>
        /// Whether or not the item was placed.
        /// </summary>
        public bool IsPlaced;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            IsPlaced = (ReadByte() & 0x1) != 0;
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            Write((IsPlaced ? 1 : 0) & 0x1);
        }
    }
}
