// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Models.Nodes;
using Spatial.Cloud.Models.Users;

namespace Spatial.Cloud.Services.Nodes;

/// <summary>
/// A factory for <see cref="Node"/> provisioning.
/// </summary>
public class Provisioner
{
    /// <summary>
    /// Provision a new <see cref="Node"/>.
    /// </summary>
    /// <param name="creator">The <see cref="User"/> provisioning the <see cref="Node"/>.</param>
    /// <returns>A <see cref="Node"/>.</returns>
    public Node Provision(string creator)
    {
        return new Node() { Creator = creator };
    }
}