// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking.Contracts.Miscellaneous;

/// <summary>
/// A <see cref="ProtocolBuffer"/> defining <see cref="NETCOMMAND.NC_MISC_SEED_CMD"/>.
/// </summary>
internal class PROTO_NC_MISC_SEED_CMD : ProtocolBuffer
{
    /// <summary>
    /// Create a new <see cref="PROTO_NC_MISC_SEED_CMD"/>.
    /// </summary>
    public PROTO_NC_MISC_SEED_CMD() {}
    
    /// <summary>
    /// Create a new <see cref="PROTO_NC_MISC_SEED_CMD"/>.
    /// </summary>
    /// <param name="seed">A seed.</param>
    public PROTO_NC_MISC_SEED_CMD(ushort seed)
    {
        Seed = seed;
    }

    /// <summary>
    /// A seed.
    /// </summary>
    public ushort Seed { get; set; }

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Seed = ReadUInt16();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Seed);
    }
}
