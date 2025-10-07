// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Models;

namespace Spatial.Cloud.Services;

public class JobFactory
{
    /// <summary>
    /// Create a new <see cref="Compute.Job"/>.
    /// </summary>
    /// <returns>A <see cref="Compute.Job"/>.</returns>
    public Job CreateJob()
    {
        return new Job();
    }
}