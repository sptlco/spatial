// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Objects;
using Spatial.Networking;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for object functions.
/// </summary>
public class ObjectController : AugmentedController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_BRIEFINFO_INFORM_CMD"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_BRIEFINFO_INFORM_CMD)]
    public void NC_BRIEFINFO_INFORM_CMD(PROTO_NC_BRIEFINFO_INFORM_CMD data)
    {
        if (_session.Ref.Map.Exists(data.nMyHnd))
        {
            _session.Ref.Focus(_session.Ref.Map.ObjectAt(data.hnd));
        }
    }
}