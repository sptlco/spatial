// Copyright © Spatial Corporation. All rights reserved.

using ILGPU.Runtime;
using Spatial.Compute.Commands;

namespace Spatial.Compute.Acceleration;

/// <summary>
/// An accelerated <see cref="CommandJob"/> that dispatches a kernel to the GPU.
/// </summary>
public abstract class KernelJob : CommandJob
{
    /// <summary>
    /// The job's <see cref="Accelerator"/>.
    /// </summary>
    protected Accelerator _accelerator;

    /// <summary>
    /// Create a new <see cref="KernelJob"/>.
    /// </summary>
    /// <param name="accelerator">The job's <see cref="Accelerator"/>.</param>
    public KernelJob(Accelerator accelerator)
    {
        _accelerator = accelerator;
    }

    /// <summary>
    /// Dispose of the <see cref="Job"/>.
    /// </summary>
    public override void Dispose()
    {
        _accelerator.Dispose();
    }
}