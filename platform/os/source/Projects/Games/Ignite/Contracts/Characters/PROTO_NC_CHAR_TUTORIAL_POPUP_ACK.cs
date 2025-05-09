// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts.Characters;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_CHAR_TUTORIAL_POPUP_ACK"/>.
/// </summary>
public class PROTO_NC_CHAR_TUTORIAL_POPUP_ACK : ProtocolBuffer
{
    /// <summary>
    /// Whether the user is choosing to opt-out of the tutorial.
    /// </summary>
    public bool bIsSkip;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        bIsSkip = ReadBoolean();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(bIsSkip);
    }
}
