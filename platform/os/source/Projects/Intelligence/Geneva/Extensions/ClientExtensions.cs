// Copyright © Spatial. All rights reserved.

using Geneva.Contracts;
using Spatial.Networking;

namespace Geneva.Extensions;

/// <summary>
/// Extension methods for <see cref="Server"/>.
/// </summary>
public static class ClientExtensions
{
    /// <summary>
    /// Issue a <see cref="NETCOMMAND"/> to the <see cref="Server"/>.
    /// </summary>
    /// <param name="client">A <see cref="Client"/>.</param>
    /// <param name="command">A <see cref="NETCOMMAND"/>.</param>
    /// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
    /// <param name="dispose">Whether or not to dispose of the <see cref="ProtocolBuffer"/>.</param>
    public static void Command(this Client client, NETCOMMAND command, ProtocolBuffer data, bool dispose = true)
    {
        client.Command((ushort) command, data, dispose);
    }
}
