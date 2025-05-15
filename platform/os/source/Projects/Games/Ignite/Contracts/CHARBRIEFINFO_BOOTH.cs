// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing character data.
/// </summary>
public class CHARBRIEFINFO_BOOTH : ProtocolBuffer
{
    /// <summary>
    /// The character's camp data.
    /// </summary>
    public CHARBRIEFINFO_CAMP camp;

    /// <summary>
    /// Whether or not the character is selling items.
    /// </summary>
    public bool issell;

    /// <summary>
    /// The booth's sign board.
    /// </summary>
    public STREETBOOTH_SIGNBOARD signboard;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        camp = Read<CHARBRIEFINFO_CAMP>();
        issell = ReadBoolean();
        signboard = Read<STREETBOOTH_SIGNBOARD>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(camp);
        Write(issell);
        Write(signboard);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        camp.Dispose();
        signboard.Dispose();

        base.Dispose();
    }
}
