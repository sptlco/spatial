// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Issue a command over the network.
/// </summary>
/// <param name="connection">The <see cref="Connection"/> executing the <see cref="Command"/>.</param>
/// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
public delegate void Command(Connection connection, ProtocolBuffer data);

/// <summary>
/// Issue an asynchronous command over the network.
/// </summary>
/// <param name="connection">The <see cref="Connection"/> executing the <see cref="Command"/>.</param>
/// <param name="data">A <see cref="ProtocolBuffer"/>.</param>

public delegate Task AsyncCommand(Connection connection, ProtocolBuffer data);