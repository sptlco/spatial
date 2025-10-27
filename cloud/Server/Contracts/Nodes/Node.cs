// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Mathematics;

namespace Spatial.Cloud.Contracts.Nodes;

/// <summary>
/// ...
/// </summary>
public class Node
{
    /// <summary>
    /// The node's identification string.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The precise <see cref="Time"/> the <see cref="Node"/> was created.
    /// </summary>
    public double Created { get; set; }

    /// <summary>
    /// The <see cref="User"/> that created the <see cref="Node"/>.
    /// </summary>
    public string Creator { get; set; }

    /// <summary>
    /// The precise location of the <see cref="Node"/>.
    /// </summary>
    public Point3D Position { get; set; }

    /// <summary>
    /// The node's current value.
    /// </summary>
    public double Value { get; set; }
}