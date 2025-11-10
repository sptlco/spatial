// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Persistence;

namespace Spatial.Cloud.Models.Compute.Jobs;

/// <summary>
/// A task executed by the <see cref="Server"/>.
/// </summary>
[Collection("jobs")]
public class Job : Record
{
    // ...
}