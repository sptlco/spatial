// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Sessions;
using Spatial.Extensions;
using Spatial.Networking;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for session functions.
/// </summary>
public class SessionController : ResponsiveController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_USER_CONNECTION_CHANGE_CHAR_ID_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_USER_CONNECTION_CHANGE_CHAR_ID_REQ)]
    public void NC_USER_CONNECTION_CHANGE_CHAR_ID_REQ(PROTO_NC_USER_CONNECTION_CHANGE_CHAR_ID_REQ data)
    {
        _session.Account.Characters.ElementAt(data.nSlotNo).Update(c => {
            c.Name = data.sNewID;
            c.Requirements.Remove(Requirements.Name);
        });

        NC_USER_CONNECTION_CHANGE_CHAR_ID_ACK(8384, data.nSlotNo, data.sNewID);
    }
}
