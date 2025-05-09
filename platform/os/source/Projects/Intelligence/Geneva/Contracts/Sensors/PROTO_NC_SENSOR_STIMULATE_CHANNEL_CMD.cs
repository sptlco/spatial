// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Geneva.Contracts.Sensors;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_SENSOR_STIMULATE_CHANNEL_CMD"/>.
/// </summary>
public class PROTO_NC_SENSOR_STIMULATE_CHANNEL_CMD : ProtocolBuffer
{
    /// <summary>
    /// A receptor channel.
    /// </summary>
    public int Channel;

    /// <summary>
    /// The intensity of the stimulus.
    /// </summary>
    public double Intensity;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Channel = ReadInt32();
        Intensity = ReadDouble();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Channel);
        Write(Intensity);
    }
}