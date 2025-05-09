// Copyright © Spatial. All rights reserved.

using Spatial.Compute.Jobs;
using Spatial.Compute.Jobs.Acceleration;
using Spatial.Compute.Jobs.Commands;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Spatial.Compute;

/// <summary>
/// A low-level abstraction that provides access to the system's 
/// central processing unit (CPU) and graphics processing unit (GPU).
/// </summary>
internal static class Processor
{
    private static Agent[] _agents;
    private static ConcurrentDictionary<long, Job> _jobs;
    private static ConcurrentDictionary<Job, StrongBox<int>> _dependencies;
    private static int _running;
    private static uint _next;

    /// <summary>
    /// The agents of the <see cref="Processor"/>.
    /// </summary>
    public static Agent[] Agents => _agents;

    /// <summary>
    /// Run the <see cref="Processor"/>.
    /// </summary>
    public static void Run()
    {
        if (Interlocked.Exchange(ref _running, 1) != 0)
        {
            return;
        }

        _agents = new Agent[System.Environment.ProcessorCount];
        _jobs = [];
        _dependencies = [];

        for (var i = 0; i < _agents.Length; i++)
        {
            _agents[i] = new(i);
        }

        for (var i = 0; i < _agents.Length; i++)
        {
            _agents[i].Run();
        }
    }

    /// <summary>
    /// Shutdown the <see cref="Processor"/>.
    /// </summary>
    internal static void Shutdown()
    {
        if (Interlocked.Exchange(ref _running, 0) != 1)
        {
            return;
        }

        for (var i = 0; i < _agents.Length; i++)
        {
            _agents[i].Dispose();
        }

        _agents = default!;
    }

    /// <summary>
    /// Submit a <see cref="Graph"/> to the <see cref="Processor"/>.
    /// </summary>
    /// <param name="graph">A <see cref="Job"/> execution <see cref="Graph"/>.</param>
    /// <returns>A <see cref="JobHandle"/>.</returns>
    public static JobHandle Dispatch(Graph graph)
    {
        // First, topologically sort the graph using Khan's algorithm
        // under the hood, ensuring there are no circular dependencies.

        if (graph.Dependencies.Count > 0)
        {
            graph.Sort();

            // To ensure that all dependencies are accounted for before 
            // jobs are submitted, we add them here.
            
            foreach (var (jobId, dependencies) in graph.Dependencies)
            {
                _dependencies[jobId] = new(dependencies.Count);
            }
        }
        
        // Finally, submit the jobs for execution.
        // Note, they won't actually be scheduled yet unless their 
        // dependencies have all been met.

        graph.Handle = JobHandle.Create(graph.Jobs.Count);
        graph.Jobs.ForEach(Submit);

        return graph.Handle;
    }

    /// <summary>
    /// Finalize a <see cref="Job"/> that was executed.
    /// </summary>
    /// <param name="jobId">The <see cref="Job"/> to finalize.</param>
    public static void Finalize(long jobId)
    {
        if (_jobs.TryRemove(jobId, out var job))
        {
            switch (job)
            {
                case BatchJob batch1D:
                    if (batch1D.Parent.Complete(batch1D.Size))
                    {
                        Finalize(batch1D.Parent.Id);
                    }

                    break;
                case Batch2DJob batch2D:
                    if (batch2D.Parent.Complete(batch2D.Size))
                    {
                        Finalize(batch2D.Parent.Id);
                    }

                    break;
                default:
                    // Notify dependents of this job (jobs that cannot run before this 
                    // job is executed) that the job has been executed, so that they can run.
                    
                    if (job.Graph.Dependants.TryGetValue(job, out var dependants))
                    {
                        foreach (var dependant in dependants)
                        {
                            if (Interlocked.Decrement(ref _dependencies[dependant].Value) <= 0 && _dependencies.TryRemove(dependant, out _))
                            {
                                Process(dependant);
                            }
                        }
                    }
                    
                    job.Graph.Handle.Signal();
                    break;
            }

            job.Dispose();
        }
    }

    private static void Submit(Job job)
    {
        _jobs[job.Id] = job;

        if (job is not BatchJob and not Batch2DJob)
        {
            if (_dependencies.TryGetValue(job, out var box) && box.Value > 0)
            {
                // If the job has dependencies that have not yet been executed,
                // defer the job's execution.
                
                return;
            }
        }
        
        Process(job);
    }

    private static void Process(Job job)
    {
        switch (job)
        {
            case CommandJob command:
                Execute(command);
                break;
            case ParallelForJob job1D:
                for (var i = 0; i < Math.Ceiling(job1D.Iterations / (double) job1D.BatchSize); i++)
                {
                    var start = i * job1D.BatchSize;
                    var end = Math.Min(start + job1D.BatchSize, job1D.Iterations);
                    var batch = BatchJob.Create(job1D, start, end);

                    Submit(batch);
                }

                break;
            case ParallelFor2DJob job2D:
                var batchesX = Math.Ceiling(job2D.Width / (double) job2D.BatchSizeX);
                var batchesY = Math.Ceiling(job2D.Height / (double) job2D.BatchSizeY);

                for (var bx = 0; bx < batchesX; bx++)
                {
                    for (var by = 0; by < batchesY; by++)
                    {
                        var startX = bx * job2D.BatchSizeX;
                        var endX = Math.Min(startX + job2D.BatchSizeX, job2D.Width);
                        var startY = by * job2D.BatchSizeY;
                        var endY = Math.Min(startY + job2D.BatchSizeY, job2D.Height);
                        var batch = Batch2DJob.Create(job2D, startX, endX, startY, endY);

                        Submit(batch);
                    }
                }

                break;
        }
    }

    private static void Execute(CommandJob job)
    {
        job.Status = JobStatus.Scheduled;

        _agents[Interlocked.Increment(ref _next) % _agents.Length].Queue.Enqueue(job);
    }
}