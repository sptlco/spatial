// Copyright © Spatial. All rights reserved.

using Ignite.Contracts.Characters;
using Spatial.Networking;

namespace Ignite.Contracts.Maps;

/// <summary>
/// A <see cref="ProtocolBuffer"/> for <see cref="NETCOMMAND.NC_MAP_LOGIN_REQ"/>.
/// </summary>
public class PROTO_NC_MAP_LOGIN_REQ : ProtocolBuffer
{
    /// <summary>
    /// The character logging in.
    /// </summary>
    public PROTO_NC_CHAR_ZONE_CHARDATA_REQ chardata;

    /// <summary>
    /// A list of file checksums.
    /// </summary>
    public string[] checksum;

    /// <summary>
    /// Deserialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Deserialize()
    {
        chardata = Read<PROTO_NC_CHAR_ZONE_CHARDATA_REQ>();
        checksum = Read<string>((int) SHN_DATA_FILE_INDEX.SHN_MaxCnt, 32);
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Serialize()
    {
        Write(chardata);
        Write(checksum, 32);
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public override void Dispose()
    {
        chardata.Dispose();

        base.Dispose();
    }
}