// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Networking;

namespace Spatial.Cloud.Contracts.Nodes;

/// <summary>
/// Raw input data sent to the server.
/// </summary>
public class Input : ProtocolBuffer
{
    /// <summary>
    /// Create a new <see cref="Input"/>.
    /// </summary>
    public Input()
    {
        Actuator = -1;
        Data = [];
    }

    /// <summary>
    /// Create a new <see cref="Input"/>.
    /// </summary>
    /// <param name="inputs">A feature vector.</param>
    public Input(int actuator, double[] data)
    {
        Actuator = actuator;
        Data = data;
    }

    /// <summary>
    /// The actuator the <see cref="Input"/> belongs to.
    /// </summary>
    public int Actuator { get; set; }

    /// <summary>
    /// Raw input data.
    /// </summary>
    public double[] Data { get; set; }

    /// <summary>
    /// Deserialize the <see cref="Input"/>.
    /// </summary>
    public override void Deserialize()
    {
        Actuator = ReadInt32();
        Data = Read<double>(ReadByte());
    }

    /// <summary>
    /// Serialize the <see cref="Input"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(Actuator);
        Write((byte) Data.Length);
        Write(Data);
    }
}