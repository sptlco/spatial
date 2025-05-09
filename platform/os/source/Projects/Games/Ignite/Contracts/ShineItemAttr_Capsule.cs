// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_Capsule : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's content.
    /// </summary>
    public SHINE_ITEM_REGISTNUMBER content;

    /// <summary>
    /// The time the item can be used.
    /// </summary>
    public ShineDateTime useabletime;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        content = Read<SHINE_ITEM_REGISTNUMBER>();
        useabletime = Read<ShineDateTime>();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(content);
        Write(useabletime);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        content.Dispose();
        useabletime.Dispose();

        base.Dispose();
    }
}
