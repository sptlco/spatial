// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing name change data.
/// </summary>
public class CHAR_ID_CHANGE_DATA : ProtocolBuffer
{
    /// <summary>
    /// Whether or not the character needs to change their name.
    /// </summary>
    public bool bNeedChangeID;

    /// <summary>
    /// Whether or not the name change was initialized.
    /// </summary>
    public bool bInit;

    /// <summary>
    /// The name change's row number.
    /// </summary>
    public uint nRowNo;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        bNeedChangeID = ReadBoolean();
        bInit = ReadBoolean();
        nRowNo = ReadUInt32();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(bNeedChangeID);
        Write(bInit);
        Write(nRowNo);
    }
}
