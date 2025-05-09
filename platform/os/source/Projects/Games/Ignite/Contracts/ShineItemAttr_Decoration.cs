// Copyright © Spatial. All rights reserved.

using Spatial.Networking;

namespace Ignite.Contracts;

/// <summary>
/// A <see cref="ProtocolBuffer"/> containing item attributes.
/// </summary>
public class ShineItemAttr_Decoration : SHINE_ITEM_ATTRIBUTE
{
    /// <summary>
    /// The item's expiration date.
    /// </summary>
    public ShineDateTime deletetime;

    /// <summary>
    /// The item's binding flags.
    /// </summary>
    public SHINE_PUT_ON_BELONGED_ITEM IsPutOnBelonged;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        deletetime = Read<ShineDateTime>();
        IsPutOnBelonged = (SHINE_PUT_ON_BELONGED_ITEM) ReadInt32();
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(deletetime);
        Write((int) IsPutOnBelonged);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        deletetime.Dispose();

        base.Dispose();
    }
}
