// Copyright © Spatial. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Issue a command over the network.
/// </summary>
/// <param name="data">A <see cref="ProtocolBuffer"/>.</param>
public delegate void Command(ProtocolBuffer data);