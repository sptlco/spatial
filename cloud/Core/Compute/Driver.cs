// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute.Commands;

namespace Spatial.Compute;

/// <summary>
/// A lightweight thread enforcing <see cref="Job"/> execution policy.
/// </summary>
public class Driver
{
    private readonly CommandJob _job;
    private readonly Thread _thread;

    /// <summary>
    /// Create a new <see cref="Driver"/>.
    /// </summary>
    /// <param name="job">A <see cref="Job"/> to execute.</param>
    public Driver(CommandJob job)
    {
        _job = job;
        _thread = new Thread(job.Execute) {
            Name = job.GetType().Name,
            Priority = ThreadPriority.Highest
        };
    }

    /// <summary>
    /// Execute the <see cref="Job"/>.
    /// </summary>
    public void Run()
    {
        _thread.Start();

        if (!_thread.Join(TimeSpan.FromMilliseconds(_job.Timeout)))
        {
            _thread.Interrupt();

            throw new Timeout($"The job exceeded the timeout of {_job.Timeout} milliseconds.");
        }
    }
}