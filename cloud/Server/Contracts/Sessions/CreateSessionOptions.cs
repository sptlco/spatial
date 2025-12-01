// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Contracts.Sessions;

/// <summary>
/// Configurable options sent to the server when creating a session.
/// </summary>
public class CreateSessionOptions
{
    /// <summary>
    /// The user creating the session.
    /// </summary>
    public string User { get; set; }
}