// Copyright © Spatial. All rights reserved.

using Ignite.Contracts;
using Ignite.Contracts.Avatars;
using Ignite.Models;
using Spatial.Networking;

namespace Ignite.Controllers;

/// <summary>
/// A <see cref="Controller"/> for avatar functions.
/// </summary>
public class AvatarController : ResponsiveController
{
    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_AVATAR_CREATE_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_AVATAR_CREATE_REQ)]
    public void NC_AVATAR_CREATE_REQ(PROTO_NC_AVATAR_CREATE_REQ data)
    {
        NC_AVATAR_CREATESUCC_ACK(_session.Account.Characters[data.slotnum] = Character.Create(
            account: _session.Account.Id,
            slot: data.slotnum,
            name: data.name,
            race: data.char_shape.race,
            @class: data.char_shape.chrclass,
            gender: data.char_shape.gender,
            hairStyle: data.char_shape.hairtype,
            hairColor: data.char_shape.haircolor,
            face: data.char_shape.faceshape));
    }

    /// <summary>
    /// Handle <see cref="NETCOMMAND.NC_AVATAR_ERASE_REQ"/>.
    /// </summary>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    [NETHANDLER(NETCOMMAND.NC_AVATAR_ERASE_REQ)]
    public void NC_AVATAR_ERASE_REQ(PROTO_NC_AVATAR_ERASE_REQ data)
    {
        _session.Account.Characters.ElementAt(data.slot).Delete();
        _session.Account.Characters.Remove(data.slot);

        NC_AVATAR_ERASESUCC_ACK(data.slot);
    }
}