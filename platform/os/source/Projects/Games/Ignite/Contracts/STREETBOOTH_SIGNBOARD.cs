// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing booth sign board data.
/// </summary>
public class STREETBOOTH_SIGNBOARD : ProtocolBuffer
{
    /// <summary>
    /// The booth's sign board.
    /// </summary>
    public string signboard;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        signboard = ReadString(30);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(signboard, 30);
    }
}
