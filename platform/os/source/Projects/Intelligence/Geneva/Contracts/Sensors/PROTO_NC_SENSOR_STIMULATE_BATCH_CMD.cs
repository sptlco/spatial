// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Geneva.Contracts.Sensors;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_SENSOR_STIMULATE_BATCH_CMD"/>.
/// </summary>
public class PROTO_NC_SENSOR_STIMULATE_BATCH_CMD : ProtocolBuffer
{
    /// <summary>
    /// The number of stimuli.
    /// </summary>
    public int Count;

    /// <summary>
    /// An array of intensity values.
    /// </summary>
    public double[] Stimuli;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Count = ReadInt32();
        Stimuli = Read<double>(Count);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Count);
        Write(Stimuli);
    }
}
