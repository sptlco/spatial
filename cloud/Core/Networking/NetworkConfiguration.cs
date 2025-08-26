// Copyright © Spatial Corporation. All rights reserved.

using System.Net;

namespace Spatial.Networking;

/// <summary>
/// Configurable options for the private network.
/// </summary>
public class NetworkConfiguration
{
    /// <summary>
    /// The network's endpoint.
    /// </summary>
    public IPEndPoint Endpoint { get; set; } = IPEndPoint.Parse(Constants.DefaultNetworkEndpoint);
}