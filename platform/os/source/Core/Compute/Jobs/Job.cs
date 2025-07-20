// Copyright © Spatial Corporation. All rights reserved.

using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;
using ILGPU.Runtime.CPU;
using Spatial.Compute.Jobs.Acceleration;
using Spatial.Compute.Jobs.Commands;

namespace Spatial.Compute.Jobs;

/// <summary>
/// A unit of work.
/// </summary>
public abstract partial class Job : IDisposable
{
    private static readonly Context Context = Context.CreateDefault();

    /// <summary>
    /// The job's identification number.
    /// </summary>
    public string Id { get; internal set; } = GenerateId();

    /// <summary>
    /// The <see cref="Jobs.Graph"/> the <see cref="Job"/> belongs to.
    /// </summary>
    internal Graph Graph { get; set; } = default!;

    /// <summary>
    /// The operational status of the <see cref="Job"/>.
    /// </summary>
    internal JobStatus Status { get; set; } = JobStatus.Submitted;

    /// <summary>
    /// Create a new <see cref="Accelerator"/>.
    /// </summary>
    /// <param name="accelerator">The type of device to use.</param>
    /// <param name="index">The index of the target device.</param>
    /// <returns>An <see cref="Accelerator"/>.</returns>
    public static Accelerator Accelerator(JobAccelerator accelerator = JobAccelerator.Auto, int index = 0)
    {
        return accelerator switch {
            JobAccelerator.Auto => Context.GetPreferredDevice(false).CreateAccelerator(Context),
            JobAccelerator.Cuda => Context.CreateCudaAccelerator(index),
            JobAccelerator.OpenCl => Context.CreateCLAccelerator(index),
            _ => Context.CreateCPUAccelerator(index),
        };
    }

    /// <summary>
    /// Dispatch a command job.
    /// </summary>
    /// <param name="action">The <see cref="Action"/> to perform.</param>
    public static void Command(Action action)
    {
        using var graph = Graph.Create().Add(CommandJob.Create(action));
        using var handle = Processor.Dispatch(graph);

        handle.Wait();
    }

    /// <summary>
    /// Dispatch a parallel for job.
    /// </summary>
    /// <param name="collection">A collection to enumerate.</param>
    /// <param name="function">The function to execute.</param>
    /// <param name="batchStrategy">The job's <see cref="BatchStrategy"/>.</param>
    public static void ParallelFor<T>(IEnumerable<T> collection, Action<T> function, BatchStrategy batchStrategy = BatchStrategy.Auto)
    {
        ParallelFor(collection.Count(), 0, (i) => function(collection.ElementAt(i)), batchStrategy);
    }

    /// <summary>
    /// Dispatch a parallel for job.
    /// </summary>
    /// <param name="collection">A collection to enumerate.</param>
    /// <param name="function">The function to execute.</param>
    /// <param name="batchStrategy">The job's <see cref="BatchStrategy"/>.</param>
    public static void ParallelFor<T>(IReadOnlyCollection<T> collection, Action<T> function, BatchStrategy batchStrategy = BatchStrategy.Auto)
    {
        ParallelFor(collection.Count, 0, (i) => function(collection.ElementAt(i)), batchStrategy);
    }

    /// <summary>
    /// Dispatch a parallel for job.
    /// </summary>
    /// <param name="collection">A collection to enumerate.</param>
    /// <param name="function">The function to execute.</param>
    /// <param name="batchStrategy">The job's <see cref="BatchStrategy"/>.</param>
    public static void ParallelFor<TKey, TValue>(IDictionary<TKey, TValue> collection, Action<TKey, TValue> function, BatchStrategy batchStrategy = BatchStrategy.Auto)
    {
        ParallelFor(collection.Count, 0, (i) => {
            var (key, value) = collection.ElementAt(i);
            function(key, value);
        }, batchStrategy);
    }

    /// <summary>
    /// Dispatch a parallel for job.
    /// </summary>
    /// <param name="count">The number of iterations to perform.</param>
    /// <param name="function">The function to execute.</param>
    /// <param name="batchStrategy">The job's <see cref="BatchStrategy"/>.</param>
    public static void ParallelFor(int count, Action<int> function, BatchStrategy batchStrategy = BatchStrategy.Auto)
    {
        ParallelFor(count, 0, function, batchStrategy);
    }

    /// <summary>
    /// Dispatch a parallel for job.
    /// </summary>
    /// <param name="iterations">The number of iterations to perform.</param>
    /// <param name="batchSize">The number of iterations to process per job.</param>
    /// <param name="function">The function to execute.</param>
    /// <param name="batchStrategy">The job's <see cref="BatchStrategy"/>.</param>
    public static void ParallelFor(int iterations, int batchSize, Action<int> function, BatchStrategy batchStrategy = BatchStrategy.Preferred)
    {
        if (iterations <= 0)
        {
            return;
        }
        
        using var graph = Graph.Create().Add(ParallelForJob.Create(iterations, batchSize, function, batchStrategy));
        using var handle = Processor.Dispatch(graph);
        
        handle.Wait();
    }

    /// <summary>
    /// Dispatch a two-dimensional parallel for job.
    /// </summary>
    /// <param name="width">The size of the first dimension.</param>
    /// <param name="height">The size of the second dimension.</param>
    /// <param name="function">The function to execute.</param>
    /// <param name="batchStrategy">The job's <see cref="BatchStrategy"/>.</param>
    public static void ParallelFor2D(int width, int height, Action<int, int> function, BatchStrategy batchStrategy = BatchStrategy.Auto)
    {
        ParallelFor2D(width, height, 0, 0, function, batchStrategy);
    }

    /// <summary>
    /// Dispatch a two-dimensional parallel for job.
    /// </summary>
    /// <param name="width">The size of the first dimension.</param>
    /// <param name="height">The size of the second dimension.</param>
    /// <param name="batchSizeX">The number of iterations to perform per job in the first dimension.</param>
    /// <param name="batchSizeY">The number of iterations to perform per job in the second dimension.</param>
    /// <param name="function">The function to execute.</param>
    /// <param name="batchStrategy">The job's <see cref="BatchStrategy"/>.</param>
    public static void ParallelFor2D(int width, int height, int batchSizeX, int batchSizeY, Action<int, int> function, BatchStrategy batchStrategy = BatchStrategy.Preferred)
    {
        if (width <= 0 || height <= 0)
        {
            return;
        }
        
        using var graph = Graph.Create().Add(ParallelFor2DJob.Create(width, height, batchSizeX, batchSizeY, function, batchStrategy));
        using var handle = Processor.Dispatch(graph);
        
        handle.Wait();
    }

    private static void Kernel(KernelJob job)
    {
        using var graph = Graph.Create().Add(job);
        using var handle = Processor.Dispatch(graph);

        handle.Wait();
    }

    private static string GenerateId()
    {
        return Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Reset the <see cref="Job"/>.
    /// </summary>
    protected void Reset()
    {
        Id = GenerateId();
        Status = JobStatus.Submitted;
    }

    /// <summary>
    /// Dispose of the <see cref="Job"/>.
    /// </summary>
    public virtual void Dispose() { }
}

/// <summary>
/// Indicates the operational status of a <see cref="Job"/>.
/// </summary>
public enum JobStatus
{
    /// <summary>
    /// The <see cref="Job"/> was submitted.
    /// </summary>
    Submitted,
    
    /// <summary>
    /// The <see cref="Job"/> has been scheduled.
    /// </summary>
    Scheduled,
    
    /// <summary>
    /// The <see cref="Job"/> is being executed.
    /// </summary>
    Running,
    
    /// <summary>
    /// The <see cref="Job"/> was executed.
    /// </summary>
    Complete,
    
    /// <summary>
    /// The system failed to execute the <see cref="Job"/>.
    /// </summary>
    Failed
}

/// <summary>
/// Specifies the device chosen by the system for execution 
/// of a <see cref="KernelJob"/>.
/// </summary>
public enum JobAccelerator
{
    /// <summary>
    /// The system will choose the most optimal device.
    /// </summary>
    Auto,

    /// <summary>
    /// The system will choose a Cuda device.
    /// </summary>
    Cuda,

    /// <summary>
    /// The system will choose an OpenCL device.
    /// </summary>
    OpenCl,

    /// <summary>
    /// The system will choose the CPU.
    /// </summary>
    CPU
}