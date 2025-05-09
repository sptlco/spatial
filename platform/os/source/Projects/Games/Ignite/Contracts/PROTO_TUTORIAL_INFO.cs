// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing tutorial data.
/// </summary>
public class PROTO_TUTORIAL_INFO : ProtocolBuffer
{
    /// <summary>
    /// The current state of the tutorial.
    /// </summary>
    public TUTORIAL_STATE nTutorialState;

    /// <summary>
    /// The current tutorial step.
    /// </summary>
    public byte nTutorialStep;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        nTutorialState = (TUTORIAL_STATE) ReadInt32();
        nTutorialStep = ReadByte();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write((int) nTutorialState);
        Write(nTutorialStep);
    }
}
