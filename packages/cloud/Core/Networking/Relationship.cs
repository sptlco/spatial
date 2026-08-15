// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// Explains one connection's relationship to another.
/// </summary>
public enum Relationship
{
    /// <summary>
    /// The <see cref="Connection"/> is a parent of the linked connection.
    /// </summary>
    Parent,

    /// <summary>
    /// The <see cref="Connection"/> is a child of the linked connection.
    /// </summary>
    Child
}
