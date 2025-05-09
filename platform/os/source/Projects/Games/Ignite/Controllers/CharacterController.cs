// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Characters;
using Spatial.Networking;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for character functions.
/// </summary>
public class CharacterController : ResponsiveController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_CHAR_LOGIN_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_CHAR_LOGIN_REQ)]
    public void NC_CHAR_LOGIN_REQ(PROTO_NC_CHAR_LOGIN_REQ data)
    {
        _session.Character = _session.Account.Characters.ElementAt(data.slot);

        // ...

        NC_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD();
        NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD();
        NC_CHAR_OPTION_IMPROVE_GET_KEYMAPPING_CMD();
        NC_CHAR_LOGIN_ACK();
    }
}
