// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Users;
using Ignite.Models;
using Spatial.Extensions;
using Spatial.Networking;
using Spatial.Persistence;
using System;
using System.Linq;

namespace Ignite.Controllers;

/// <summary>
/// A controller for user functions.
/// </summary>
public class UserController : ResponsiveController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_USER_CLIENT_VERSION_CHECK_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_USER_CLIENT_VERSION_CHECK_REQ)]
    public void NC_USER_CLIENT_VERSION_CHECK_REQ(PROTO_NC_USER_CLIENT_VERSION_CHECK_REQ data)
    {
        if (!Constants.Versions.Any(data.sVersionKey.Matches))
        {
            NC_USER_CLIENT_WRONGVERSION_CHECK_ACK();
            return;
        }

        NC_USER_CLIENT_RIGHTVERSION_CHECK_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_USER_XTRAP_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_USER_XTRAP_REQ)]
    public void NC_USER_XTRAP_REQ(PROTO_NC_USER_XTRAP_REQ _)
    {
        NC_USER_XTRAP_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_USER_US_LOGIN_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_USER_US_LOGIN_REQ)]
    public void NC_USER_US_LOGIN_REQ(PROTO_NC_USER_US_LOGIN_REQ data)
    {
        var account = Document<Account>.FirstOrDefault(a => a.Username == data.sUserName && a.Password == data.sPassword);

        if (account == null)
        {
#if DEBUG
            account = Account.Create(data.sUserName, data.sPassword);
#else
            NC_USER_LOGINFAIL_ACK(69);
            return;
#endif
        }

        Authenticate(Session.Create(account.Load()).Reference());
        NC_USER_LOGIN_ACK();
    }

    /// <summary>
    /// Get the current status of all worlds.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_USER_WORLD_STATUS_REQ)]
    public void NC_USER_WORLD_STATUS_REQ(PROTO_NC_USER_WORLD_STATUS_REQ _)
    {
        NC_USER_WORLD_STATUS_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_USER_NORMALLOGOUT_CMD"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_USER_NORMALLOGOUT_CMD)]
    public void NC_USER_NORMALLOGOUT_CMD(PROTO_NC_USER_NORMALLOGOUT_CMD _) 
    {
        if (_connection == _session.Connection.Map)
        {
            if (_session.Player.HasValue)
            {
                _session.Map.Destroy(_session.Player.Value);
            }

            _session.Character?.Save();
            _session.Character = null!;
        }
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_USER_WORLDSELECT_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_USER_WORLDSELECT_REQ)]
    public void NC_USER_WORLDSELECT_REQ(PROTO_NC_USER_WORLDSELECT_REQ _)
    {
        // This is unsafe, but it allows us to reuse the session.
        // A case that should be considered, though, is where the connection 
        // drops and is never reestablished, leaving the session alive.

        _session.Reference();

        NC_USER_WORLDSELECT_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_USER_LOGINWORLD_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_USER_LOGINWORLD_REQ)]
    public void NC_USER_LOGINWORLD_REQ(PROTO_NC_USER_LOGINWORLD_REQ data)
    {
        Monitor().Authenticate(Session.Decode(data.validate_new));

        _session.Connection.World = _connection;

        NC_USER_LOGINWORLD_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_USER_WILL_WORLD_SELECT_REQ"/>.
    /// </summary>
    /// <param name="_">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_USER_WILL_WORLD_SELECT_REQ)]
    public void NC_USER_WILL_WORLD_SELECT_REQ(PROTO_NC_USER_WILL_WORLD_SELECT_REQ _)
    {
        NC_USER_WILL_WORLD_SELECT_ACK();
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_USER_LOGIN_WITH_OTP_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_USER_LOGIN_WITH_OTP_REQ)]
    public void NC_USER_LOGIN_WITH_OTP_REQ(PROTO_NC_USER_LOGIN_WITH_OTP_REQ data)
    {
        try
        {
            Authenticate(Session.FindOrDefault(data.sOTP) ?? throw new ArgumentException("Invalid one-time password provided."));
        }
        catch (ArgumentException)
        {
            NC_USER_LOGINFAIL_ACK(73);
            return;
        }

        NC_USER_LOGIN_ACK();
    }
}