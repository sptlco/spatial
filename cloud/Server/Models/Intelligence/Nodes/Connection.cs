// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Models.Intelligence.Nodes;

/// <summary>
/// A weighted connection between two nodes.
/// </summary>
[Collection("connections")]
public class Connection : Record
{
    /// <summary>
    /// The <see cref="Node"/> the <see cref="Connection"/> extends from.
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// The <see cref="Node"/> the <see cref="Connection"/> extends to.
    /// </summary>
    public string To { get; set; }

    /// <summary>
    /// The strength of the <see cref="Connection"/>.
    /// </summary>
    public double Strength { get; set; }
}
