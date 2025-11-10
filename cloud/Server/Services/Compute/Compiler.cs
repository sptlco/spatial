// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Models.Compute.Jobs;

namespace Spatial.Cloud.Services.Compute;

/// <summary>
/// A factory for <see cref="Job"/> compilation.
/// </summary>
public class Compiler
{
    /// <summary>
    /// Compile a new <see cref="Graph"/>.
    /// </summary>
    /// <returns>A <see cref="Graph"/>.</returns>
    public Graph Compile()
    {
        return new Graph();
    }
}