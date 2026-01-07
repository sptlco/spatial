// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Principals;

/// <summary>
/// An identity for the current request used for all authorization decisions.
/// </summary>
public class Principal
{
    /// <summary>
    /// The user's roles.
    /// </summary>
    public List<string> Roles { get; set; }

    /// <summary>
    /// The user's permissions.
    /// </summary>
    public List<string> Permissions { get; set; }
}