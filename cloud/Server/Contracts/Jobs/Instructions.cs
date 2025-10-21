// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Contracts.Jobs;

/// <summary>
/// Instructions for <see cref="Job"/> execution.
/// </summary>
public class Instructions
{
    /// <summary>
    /// A unique identifier for the <see cref="Job"/>.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// A list of jobs the <see cref="Job"/> depends on.
    /// </summary>
    public List<string> Dependencies { get; set; } = [];

    // ...
}