// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Nodes;
using Spatial.Cloud.Services.Nodes;
using Spatial.Mathematics;
using Spatial.Networking;

namespace Spatial.Cloud.Controllers.Nodes;

/// <summary>
/// A <see cref="Controller"/> for <see cref="Node"/>.
/// </summary>
[Module]
[Path("nodes")]
public class NodeController : Controller
{
    private readonly Provisioner _provisioner;

    /// <summary>
    /// Create a new <see cref="NodeController"/>.
    /// </summary>
    public NodeController()
    {
        _provisioner = new Provisioner();
    }

    /// <summary>
    /// Create a new <see cref="Node"/>.
    /// </summary>
    /// <param name="location">The <see cref="Point3D"/> at which to place the <see cref="Node"/>.</param>
    /// <returns>A <see cref="Node"/>.</returns>
    [POST]
    [Path("/")]
    public async Task<Node> CreateNodeAsync()
    {
        var node = _provisioner.Provision(string.Empty);

        // ...

        return await Task.FromResult(new Node {
            Id = node.Id,
            Created = node.Created,
            Creator = node.Creator,
            Position = node.Position,
            Value = node.Value
        });
    }
}