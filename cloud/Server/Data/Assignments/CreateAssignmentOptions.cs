// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Assignments;

/// <summary>
/// Configurable options for a role assignment.
/// </summary>
public class CreateAssignmentOptions
{
    /// <summary>
    /// A user identifier.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// A role identifier.
    /// </summary>
    public string Role { get; set; }
}