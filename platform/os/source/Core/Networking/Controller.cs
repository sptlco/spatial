// Copyright © Spatial. All rights reserved.

namespace Spatial.Networking;

/// <summary>
/// A device through which a client communicates with the <see cref="Server"/>.
/// </summary>
public abstract class Controller
{
    /// <summary>
    /// The active <see cref="Connection"/>.
    /// </summary>
    public Connection _connection { get; internal set; }

    /// <summary>
    /// Initialize the <see cref="Controller"/>.
    /// </summary>
    public virtual void Initialize(ushort command)
    {
        // Initialization logic can be added here if needed.
    }
}