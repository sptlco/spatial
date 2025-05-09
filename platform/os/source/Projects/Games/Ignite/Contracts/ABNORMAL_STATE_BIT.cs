// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing abnormal state bits.
/// </summary>
public class ABNORMAL_STATE_BIT : ProtocolBuffer
{
    /// <summary>
    /// Abnormal state bits.
    /// </summary>
    public byte[] statebit;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        statebit = ReadBytes(Constants.AbnormalStateBits);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(statebit);
    }
}
