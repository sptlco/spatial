// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Geneva.Contracts.Motors;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_MOTOR_OUTPUT_CMD"/>.
/// </summary>
public class PROTO_NC_MOTOR_OUTPUT_CMD : ProtocolBuffer
{
    /// <summary>
    /// The number of motors represented.
    /// </summary>
    public int Count;

    /// <summary>
    /// A list of motor outputs.
    /// </summary>
    public double[] Outputs;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        Count = ReadInt32();
        Outputs = Read<double>(Count);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Count);
        Write(Outputs);
    }
}
