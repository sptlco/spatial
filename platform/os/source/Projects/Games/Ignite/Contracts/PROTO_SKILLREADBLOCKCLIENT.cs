// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing skill data.
/// </summary>
public class PROTO_SKILLREADBLOCKCLIENT : ProtocolBuffer
{
    /// <summary>
    /// The skill's identification number.
    /// </summary>
    public ushort skillid;

    /// <summary>
    /// The skill's cool time.
    /// </summary>
    public uint cooltime;

    /// <summary>
    /// The skill's empowerment.
    /// </summary>
    public SKILL_EMPOWER empow;

    /// <summary>
    /// The skill's mastery level.
    /// </summary>
    public uint mastery;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        skillid = ReadUInt16();
        cooltime = ReadUInt32();
        empow = Read<SKILL_EMPOWER>();
        mastery = ReadUInt32();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(skillid);
        Write(cooltime);
        Write(empow);
        Write(mastery);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        empow.Dispose();

        base.Dispose();
    }

    /// <summary>
    /// A <see cref="ProtocolBuffer"/> containing skill empowerment data.
    /// </summary>
    public class SKILL_EMPOWER : ProtocolBuffer
    {
        /// <summary>
        /// Points empowering skill damage.
        /// </summary>
        public byte damage;

        /// <summary>
        /// Points empowering skill SP consumption.
        /// </summary>
        public bool sp;

        /// <summary>
        /// Points empowering skill effect keep time.
        /// </summary>
        public byte keeptime;

        /// <summary>
        /// Points empowering skill cool time.
        /// </summary>
        public byte cooltime;

        /// <summary>
        /// Deserialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Deserialize()
        {
            var e1 = ReadByte();
            var e2 = ReadByte();

            // ...
        }

        /// <summary>
        /// Serialize the <see cref="ProtocolBuffer"/>.
        /// </summary>
        public override void Serialize()
        {
            byte e1 = 0;
            byte e2 = 0;

            // ...

            Write(e1);
            Write(e2);
        }
    }
}
