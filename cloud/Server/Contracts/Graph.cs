// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Contracts;

/// <summary>
/// A <see cref="Job"/> execution graph.
/// </summary>
public class Graph
{
    /// <summary>
    /// The jobs executed by the <see cref="Graph"/>.
    /// </summary>
    public List<Job> Jobs { get; set; }
}
